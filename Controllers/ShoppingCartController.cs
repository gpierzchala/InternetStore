using System;
using System.Linq;
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
        private readonly HttpCookie _itemCount;
        private readonly HttpCookie _summaryPrice;
        private readonly Common commonDeliverTypes;

        public ShoppingCartController(
            IProductsRepository productRepo,
            ICategoryRepository catRepo,
            IDeliveryTypesRepository deliverTypesRepo,
            IManufacturersRepository manufacturersRepository)
            : base(catRepo, manufacturersRepository,productRepo)
        {
           _deliveryTypesRepository = deliverTypesRepo;
            commonDeliverTypes = new Common(_deliveryTypesRepository);

            ViewBag.Categories = GetCategories();
            ViewBag.CategorySelectList = GetCategoryList();
            ViewBag.Manufacturers = GetManufacturers();
            ViewBag.DeliverTypes = commonDeliverTypes.GetDeliverTypes();
            ViewData["SearchOptions"] = new SelectList(Common.GetSortOptions(), "Id", "Name", 1);

            if (_summaryPrice == null)
            {
                _summaryPrice = new HttpCookie("summary");
            }
            if (_itemCount == null)
            {
                _itemCount = new HttpCookie("count");
            }
        }


        public ActionResult MyCart()
        {
            ShoppingCartModel cart = ShoppingCartModel.GetCart(HttpContext);
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
            Products product = _productsRepository.Get(id);

            ShoppingCartModel shoppingCartModel = ShoppingCartModel.GetCart(HttpContext);

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
            ShoppingCartModel cart = ShoppingCartModel.GetCart(HttpContext);

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
            ShoppingCartModel userCartItems = ShoppingCartModel.GetCart(HttpContext);

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
            ShoppingCartModel cartItems = ShoppingCartModel.GetCart(HttpContext);
            DeliveryTypes delivery = _deliveryTypesRepository.Get(deliveryType);

            if (cartItems == null || delivery == null)
                return View("_Error");

            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cartItems.GetCartItems(),
                CartTotal = cartItems.GetTotal() + delivery.Price,
                DeliveryType = delivery.Name,
                DeliveryCost = delivery.Price
            };
            return View("CartSummary", viewModel);
        }

        [HttpPost]
        public ActionResult ChangeCount(int id, int count)
        {
            ShoppingCartModel shoppingCartModel = ShoppingCartModel.GetCart(HttpContext);

            if (shoppingCartModel == null)
                return View("_Error");

            ShoppingCarts item = shoppingCartModel.GetCartItems().FirstOrDefault(x => x.ID == id);
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
    }
}