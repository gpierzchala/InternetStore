using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;
using SimpleCrypto;
using SklepInternetowy.Helpers;

namespace SklepInternetowy.Controllers
{
    public class SetupController : Controller
    {
        private readonly ICategoryRepository _catRepo;
        private readonly IManufacturersRepository _manuRepo;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly IProductsRepository _productRepo;
        private readonly ISession _session;
        private readonly IUserRepository _userRepo;
        private readonly IDeliveryTypesRepository _deliveryRepo;
        private readonly IOrderStateRepository _orderStateRepository;

        public SetupController(ICategoryRepository catRepo, IOrderDetailsRepository orderDetailsRepository,
            IOrdersRepository orderRepo, IManufacturersRepository manuRepo,
            IProductImagesRepository productImgRepo, IUserRepository userRepo, INhibernateConnection conn,
            IProductsRepository productRepo,
            IDeliveryTypesRepository deliveryRepo, IOrderStateRepository orderStateRepository)
        {
            _manuRepo = manuRepo;
            _productRepo = productRepo;
            _catRepo = catRepo;
            _productImagesRepository = productImgRepo;
            _userRepo = userRepo;
            _ordersRepository = orderRepo;
            _orderDetailsRepository = orderDetailsRepository;
            _deliveryRepo = deliveryRepo;
            _orderStateRepository = orderStateRepository;
            _session = conn.CreateSessionFactory().OpenSession();
        }


        public ActionResult Index()
        {
            _session.CreateSQLQuery("delete orderdetails").ExecuteUpdate();
            _session.CreateSQLQuery("delete shoppingcarts").ExecuteUpdate();
            _session.CreateSQLQuery("delete productimages").ExecuteUpdate();
            _session.CreateSQLQuery("delete orders").ExecuteUpdate();
            _session.CreateSQLQuery("delete  users").ExecuteUpdate();
            _session.CreateSQLQuery("delete products").ExecuteUpdate();
            _session.CreateSQLQuery("delete orderstate").ExecuteUpdate();
            _session.CreateSQLQuery("delete deliverytypes").ExecuteUpdate();
            _session.CreateSQLQuery("delete categories").ExecuteUpdate();
            _session.CreateSQLQuery("delete  manufacturers").ExecuteUpdate();

            #region OrderStates

            var orderState1 = new OrderState("Nowe zamówienie");
            var orderState2 = new OrderState("W trakcie realizacji");
            var orderState3 = new OrderState("Przekazano do doręczenia");

            #endregion

            #region manufacturers

            var manufacturer = new Manufacturers("Electronic Arts");
            var manufacturer2 = new Manufacturers("Ubisoft");
            var manufacturer3 = new Manufacturers("CD Projekt RED");
            var manufacturer4 = new Manufacturers("Blizzard");
            var manufacturer5 = new Manufacturers("SEGA");
            var manufacturer6 = new Manufacturers("RockstarGames");

            #endregion

            #region catregories

            var category1 = new Categories("Akcja", "");
            var category2 = new Categories("MMORpg", "Massively multiplayer online role-playing game");
            var category3 = new Categories("Sport", "");
            var category4 = new Categories("Klasyka", "");
            var category5 = new Categories("Pozostałe", "");

            #endregion

            #region products

            var productList = new List<Products>
            {
                new Products("Fifa", "", randomDecimal(5, 150), category3,
                    manufacturer, 15, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Need for speed", "", randomDecimal(5, 150), category3,
                    manufacturer, 15, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("The sims", "", randomDecimal(5, 150), category5,
                    manufacturer, 15, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Battlefield", "", randomDecimal(5, 150), category4,
                    manufacturer, 15, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Medal of honor", "", randomDecimal(5, 150), category4,
                    manufacturer, 15, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Watch dogs", "", randomDecimal(5, 150), category1,
                    manufacturer2, 5, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Far cry", "", randomDecimal(5, 150), category1,
                    manufacturer2, 12, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Heroes V", "", randomDecimal(5, 150), category4,
                    manufacturer2, 1, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Assassins Creed", "", randomDecimal(5, 150), category5,
                    manufacturer2, 4, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Wiedźmin", "", randomDecimal(5, 150), category1,
                    manufacturer3, 25, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Total war: Rome II", "", randomDecimal(5, 150), category1,
                    manufacturer4, 13, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Company of heroes", "", randomDecimal(5, 150), category4,
                    manufacturer4, 9, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("GTA", "", randomDecimal(5, 150), category5,
                    manufacturer5, 13, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), ""),
                new Products("Max payne", "", randomDecimal(5, 150), category4,
                    manufacturer5, 9, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), "")
            };

            #endregion

            #region productImages

            string filename = Server.MapPath(Url.Content("~/FrontEnd/img/empty_gallery.png"));
            byte[] bytes = System.IO.File.ReadAllBytes(filename);
            var products = _productRepo.GetAll();

            #endregion

            #region users

            const string password = "P@ssw0rd";
            var crypto = new PBKDF2();
            string enryptPass = crypto.Compute(password);
            string passwordSalt = crypto.Salt;

            const string email = "user@email.com";
            const string city = "Warszawa";
            const string address = "Sik 41/12";
            const bool isAdmin = false;
            const string userName = "Name";
            const string surname = "Surname";
            const string ipAddress = "102.154.12.12";
            const string zipCode = "12-222";


            var user = new Users(userName, surname, email, enryptPass, city, address, zipCode, isAdmin,
                passwordSalt, ipAddress);

            var user2 = new Users(userName, surname, "admin@outlook.com", enryptPass, "Łódź", address, zipCode, true,
                passwordSalt, ipAddress);

            var user3 = new Users(userName, surname, "user2@email.com", enryptPass, "Katowice", address, zipCode,
                isAdmin,
                passwordSalt, ipAddress);

            #endregion

            #region deliveryTypes

            var deliveryType = new DeliveryTypes("Poczta", Convert.ToDecimal(8.99));
            var deliveryType2 = new DeliveryTypes("Kurier", Convert.ToDecimal(12.00));
            var deliveryType3 = new DeliveryTypes("Obiór osobisty", Convert.ToDecimal(0.00));

            #endregion

            _catRepo.Save(category1);
            _catRepo.Save(category2);
            _catRepo.Save(category3);
            _catRepo.Save(category4);
            _catRepo.Save(category5);

            _orderStateRepository.Save(orderState1);
            _orderStateRepository.Save(orderState2);
            _orderStateRepository.Save(orderState3);

            _manuRepo.Save(manufacturer);
            _manuRepo.Save(manufacturer2);
            _manuRepo.Save(manufacturer3);
            _manuRepo.Save(manufacturer5);
            _manuRepo.Save(manufacturer4);
            _manuRepo.Save(manufacturer6);

            _userRepo.Save(user);
            _userRepo.Save(user2);
            _userRepo.Save(user3);
            foreach (var item in productList)
            {
                item.Description =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse sem mi, efficitur eget nisi vitae, facilisis efficitur massa. Sed rhoncus vestibulum velit, sit amet sodales nisl semper id. Praesent non nisi vitae orci facilisis dapibus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aliquam auctor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse sem mi, efficitur eget nisi vitae, facilisis efficitur massa. Sed rhoncus vestibulum velit, sit amet sodales nisl semper id. Praesent non nisi vitae orci facilisis dapibus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aliquam auctor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse sem mi, efficitur eget nisi vitae, facilisis efficitur massa. Sed rhoncus vestibulum velit, sit amet sodales nisl semper id. Praesent non nisi vitae orci facilisis dapibus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aliquam auctor.";
                item.ShortDescription =
                    "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse sem mi, efficitur eget nisi vitae, facilisis efficitur massa. Sed rhoncus vestibulum velit, sit amet sodales nisl semper id. Praesent non nisi vitae orci facilisis dapibus. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Aliquam auctor.";
                _productRepo.Save(item);
            }
            foreach (var productse in products)
                _productImagesRepository.Save(new ProductImages(filename, bytes, productse));

            _deliveryRepo.Save(deliveryType);
            _deliveryRepo.Save(deliveryType2);
            _deliveryRepo.Save(deliveryType3);

            Random rnd = new Random();
            for (int i = 0; i < 150; i++)
            {
                System.Threading.Thread.Sleep(10);
                var order1 = new Orders(user, DateTime.Now.AddDays(-rnd.Next(1, 365)),
                    Convert.ToDecimal(rnd.Next(50, 400)), deliveryType, orderState1);
                var order2 = new Orders(user2, DateTime.Now.AddDays(-rnd.Next(1, 365)),
                    Convert.ToDecimal(rnd.Next(50, 400)), deliveryType, orderState2);
                var order3 = new Orders(user3, DateTime.Now.AddDays(-rnd.Next(1, 365)),
                    Convert.ToDecimal(rnd.Next(50, 400)), deliveryType, orderState2);
                _ordersRepository.Save(order1);
                _ordersRepository.Save(order2);
                _ordersRepository.Save(order3);
                var random = rnd.Next(1, productList.Count);
                var orderDetails = new OrderDetails(order1, productList[random], rnd.Next(1, 5),
                    productList[random].Price);
                var orderDetails2 = new OrderDetails(order2, productList[random], rnd.Next(1, 5),
                    productList[random].Price);
                var orderDetails3 = new OrderDetails(order3, productList[random], rnd.Next(1, 5),
                    productList[random].Price);

                _orderDetailsRepository.Save(orderDetails);
                _orderDetailsRepository.Save(orderDetails2);
                _orderDetailsRepository.Save(orderDetails3);
            }


            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            FormsAuthentication.SignOut();


            return RedirectToAction("index", "Home");
        }

        private static decimal randomDecimal(double start, double end)
        {
            var rand = new Random();
            return Convert.ToDecimal(rand.NextDouble()*Math.Abs(end - start) + start);
        }
    }
}