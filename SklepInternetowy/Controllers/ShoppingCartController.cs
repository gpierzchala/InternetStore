using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using SklepInternetowy.Helpers;
using SklepInternetowy.Models;


namespace SklepInternetowy.Controllers
{
    [Authorize]
    public class ShoppingCartController : BaseController
    {
        private readonly IDeliveryTypesRepository _deliveryTypesRepository;
        private readonly IUserRepository _userRepository;
        private readonly HttpCookie _itemCount;
        private readonly HttpCookie _summaryPrice;
        private readonly Common _commonDeliverTypes;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IOrderStateRepository _orderStateRepository;

        public ShoppingCartController(
            IProductsRepository productRepo,
            ICategoryRepository catRepo,
            IDeliveryTypesRepository deliverTypesRepo,
            IManufacturersRepository manufacturersRepository,
            IUserRepository userRepository,
            IOrderDetailsRepository orderDetailsRepository, IOrderStateRepository orderStateRepository)
            : base(catRepo, manufacturersRepository, productRepo)
        {
            _deliveryTypesRepository = deliverTypesRepo;
            _userRepository = userRepository;
            _orderDetailsRepository = orderDetailsRepository;
            _orderStateRepository = orderStateRepository;
            _commonDeliverTypes = new Common(_deliveryTypesRepository);

            ViewBag.Categories = GetCategories();
            ViewBag.CategorySelectList = GetCategoryList();
            ViewBag.Manufacturers = GetManufacturers();
            ViewBag.DeliverTypes = _commonDeliverTypes.GetDeliverTypes();
            ViewData["SearchOptions"] = new SelectList(Common.GetSortOptions(), "Id", "Name", 1);

            if (_summaryPrice == null)
                _summaryPrice = new HttpCookie("summary");

            if (_itemCount == null)
                _itemCount = new HttpCookie("count");
        }


        public ActionResult MyCart()
        {
            var cart = ShoppingCartModel.GetCart(HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal(),
                ItemCount = cart.GetCount()
            };

            return View(viewModel);
        }

        public ActionResult AddToCart(int id)
        {
            var product = ProductsRepository.Get(id);
            var shoppingCartModel = ShoppingCartModel.GetCart(HttpContext);

            if (product == null || shoppingCartModel == null)
                return View(("_Error"));

            shoppingCartModel.AddToCart(product);
            _summaryPrice.Value = shoppingCartModel.GetTotal().ToString("F");
            Response.Cookies.Set(_summaryPrice);
            _itemCount.Value = shoppingCartModel.GetCount().ToString();
            Response.Cookies.Set(_itemCount);

            return RedirectToAction("MyCart", "ShoppingCart");
        }

        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCartModel.GetCart(HttpContext);

            if (cart == null)
                return View("_Error");

            cart.RemoveFromCart(id);
            _summaryPrice.Value = cart.GetTotal().ToString("F");
            Response.Cookies.Set(_summaryPrice);
            _itemCount.Value = cart.GetCount().ToString();
            Response.Cookies.Set(_itemCount);

            return RedirectToAction("MyCart");
        }

        public ActionResult CleanCart()
        {
            var userCartItems = ShoppingCartModel.GetCart(HttpContext);

            if (userCartItems == null)
                return View("_Error");

            userCartItems.EmptyCart();

            _summaryPrice.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(_summaryPrice);
            _itemCount.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(_itemCount);


            return RedirectToAction("MyCart");
        }

        [HttpPost]
        public ActionResult CartSummary(int deliveryType)
        {
            var cartItems = ShoppingCartModel.GetCart(HttpContext);
            var delivery = _deliveryTypesRepository.Get(deliveryType);

            if (cartItems == null || delivery == null)
                return View("_Error");

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cartItems.GetCartItems(),
                CartTotal = cartItems.GetTotal() + delivery.Price,
                DeliveryType = delivery.Name,
                DeliveryCost = delivery.Price,
                DeliveryId = deliveryType
            };
            return View("CartSummary", viewModel);
        }

        [HttpPost]
        public ActionResult ChangeCount(int id, int count)
        {
            ShoppingCartModel shoppingCartModel = ShoppingCartModel.GetCart(HttpContext);

            if (shoppingCartModel == null)
                return View("_Error");

            var item = shoppingCartModel.GetCartItems().FirstOrDefault(x => x.ID == id);
            if (item != null)
            {
                shoppingCartModel.UpdateItem(id, count);
                _summaryPrice.Value = shoppingCartModel.GetTotal().ToString("F");
                Response.Cookies.Set(_summaryPrice);
                _itemCount.Value = shoppingCartModel.GetCount().ToString();
                Response.Cookies.Set(_itemCount);
            }

            return RedirectToAction("MyCart");
        }

        [HttpPost]
        public void ProceedOrder(int delivery)
        {
            var cart = ShoppingCartModel.GetCart(HttpContext);
            var user = _userRepository.Get(User.Identity.Name);

            var selectedDelivery = _deliveryTypesRepository.Get(delivery);

            var order = new Orders(user, DateTime.Now, cart.GetTotal(), selectedDelivery, _orderStateRepository.Get(1));
            cart.CreateOrder(order);
            var orderDetails = _orderDetailsRepository.GetAll().Where(x => x.Order.Id == order.Id).ToList();
            SendEmail(order, orderDetails);
        }


        public ActionResult OrderSuccess()
        {
            return View("OrderSuccess");
        }

        public ActionResult OrderFails()
        {
            return View("OrderFails");
        }

        private ActionResult SendEmail(Orders order, IEnumerable<OrderDetails> orderDetails)
        {
            var user = _userRepository.Get(User.Identity.Name);
            var smtp = new SmtpClient("smtp.gmail.com") {UseDefaultCredentials = false};
            var credentials = new System.Net.NetworkCredential("xsamesxshop@gmail.com", "XGamesXShop");
            smtp.Credentials = credentials;
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            var message = new StringBuilder("Witaj <b>" + user.Name + " " + user.Surname + " !</b><br/>");
            message.Append(
                "W naszym serwisie dokonałeś zakupu poniższych produktów. Na poniższe dane należy przelać kwotę <b>" +
                order.SummaryPrice.ToString("C") + "</b>. Po zaksięgowaniu wpłaty towar zostanie niezwłocznie wysłany. " +
                "Poniżej znajdziesz szczegółowe dane Twoich zakupów.<br/><br/>");
            message.Append("<b>Dane do przelewu:</b><br/><br/>");

            message.Append(
                "SklepInternegowy.pl<br/>Grzegorz Pierzchała<br />ul. Lewartowskiego 5,<br />31 -234 Warszawa<br />Numer rachunku: 1234 5678 9012 3456<br/><b>W Tytule przelewu : " +
                "Zamówienie nr " + order.Id);
            message.Append("<br/>");
            message.Append("<hr/>");
            message.Append("<b>Dane transakcji</b><br/><hr/>");

            message.Append("<b> Sposób dostawy: </b>" + order.DeliveryType.Name + "<br/>");
            message.Append("<b> Kosz dostawy: </b>" + order.DeliveryType.Price.ToString("c") + "<br/>");
            message.Append("<b> Całkowity koszt zamówienia: </b>" + order.SummaryPrice.ToString("c") + "<br/></br>");

            message.Append("<b>Zakupione przedmioty:</b></br></br>");
            message.Append("<table align='center' border='1'><thead>" +
                           "<th>Produkt</th><th>Ilość</th><th>Cena jednostkowa</th><th>Cena całkowita</th>");

            foreach (var orderDetailse in orderDetails)
                message.Append("<tr><td>" + orderDetailse.Product.Name + "</td><td>" + orderDetailse.Quantity +
                               "</td><td>" + orderDetailse.UnitPrice.ToString("c") + "</td><td>" +
                               (orderDetailse.UnitPrice*orderDetailse.Quantity).ToString("c") + "</td></tr>");

            message.Append("</thead></table>");

            var mailMessage = new MailMessage()
            {
                Body = message.ToString(),
                IsBodyHtml = true,
                From = new MailAddress("shop@xGames.com")
            };
            mailMessage.To.Add(order.User.Email);
            mailMessage.Subject = "Zamówienie nr " + order.Id + " z dnia " + order.OrderDate.ToShortDateString();
            smtp.Send(mailMessage);

            return RedirectToAction("OrderSuccess");
        }
    }
}