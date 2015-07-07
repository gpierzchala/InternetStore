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

        public SetupController(ICategoryRepository catRepo, IOrderDetailsRepository orderDetailsRepository,
            IOrdersRepository orderRepo, IManufacturersRepository manuRepo,
            IProductImagesRepository productImgRepo, IUserRepository userRepo, INhibernateConnection conn,
            IProductsRepository productRepo,
            IDeliveryTypesRepository deliveryRepo)
        {
            _manuRepo = manuRepo;
            _productRepo = productRepo;
            _catRepo = catRepo;
            _productImagesRepository = productImgRepo;
            _userRepo = userRepo;
            _ordersRepository = orderRepo;
            _orderDetailsRepository = orderDetailsRepository;
            _deliveryRepo = deliveryRepo;
            _session = conn.CreateSessionFactory().OpenSession();
        }


        public ActionResult InstallDB()
        {
            _session.CreateSQLQuery("delete from orderdetails").ExecuteUpdate();
            _session.CreateSQLQuery("delete from orders").ExecuteUpdate();
            _session.CreateSQLQuery("delete from productimages").ExecuteUpdate();
            _session.CreateSQLQuery("delete from products").ExecuteUpdate();
            _session.CreateSQLQuery("delete from categories").ExecuteUpdate();
            _session.CreateSQLQuery("delete from  manufacturers").ExecuteUpdate();
            _session.CreateSQLQuery("delete from  users").ExecuteUpdate();
            _session.CreateSQLQuery("delete from deliverytypes").ExecuteUpdate();

            #region catregorie

            var category1 = new Categories("Akcja","");
            var category2 = new Categories("MMO", "");
            var category3 = new Categories("Zręcznościowe", "");
            var category4 = new Categories("Klasyka", "");
            var category5 = new Categories("Strategia", "");
            var category6 = new Categories("Logiczne", "");
            var category7 = new Categories("Bijatyki", "");
            var category8 = new Categories("Sportowe", "");
            var category9 = new Categories("Wyścigi i rajdy", "");
            var category10 = new Categories("Symulatory", "");

            _catRepo.Save(category1);
            _catRepo.Save(category2);
            _catRepo.Save(category3);
            _catRepo.Save(category4);
            _catRepo.Save(category5);
            _catRepo.Save(category6);
            _catRepo.Save(category7);
            _catRepo.Save(category8);
            _catRepo.Save(category9);
            _catRepo.Save(category10);

            #endregion

            #region producenci

            var manufacturer = new Manufacturers("Electronic Arts");
            var manufacturer2 = new Manufacturers("Ubisoft");
            var manufacturer3 = new Manufacturers("CD Projekt RED");
            var manufacturer4 = new Manufacturers("Blizzard");
            var manufacturer5 = new Manufacturers("SEGA");
            var manufacturer6 = new Manufacturers("Valve");
            var manufacturer7 = new Manufacturers("BioVare");
            var manufacturer8 = new Manufacturers("Activision");
            var manufacturer9 = new Manufacturers("RockstarGames");
            var manufacturer10 = new Manufacturers("THQ");

            _manuRepo.Save(manufacturer);
            _manuRepo.Save(manufacturer2);
            _manuRepo.Save(manufacturer3);
            _manuRepo.Save(manufacturer5);
            _manuRepo.Save(manufacturer4);
            _manuRepo.Save(manufacturer6);
            _manuRepo.Save(manufacturer7);
            _manuRepo.Save(manufacturer8);
            _manuRepo.Save(manufacturer9);
            _manuRepo.Save(manufacturer10);

            #endregion

            #region products
            
            Random rand = new Random();
            for (int i = 0; i < 30; i++)
            {
                string name = "Test " + i;
                string description = "Description " + i;
                string shortDescription = "Short desc " + i;
                var randomPrice = rand.Next(20, 200);
                var category = rand.Next(category1.ID, category6.ID+1);

                var producXt = new Products(name, description, Convert.ToDecimal(randomPrice), _catRepo.Get(category),
                    _manuRepo.Get(category), category, Common.GetRandomBool(), Common.GetRandomBool(),
                    Common.GetRandomBool(), shortDescription);

                _productRepo.Save(producXt);
            }
            
            #endregion

            #region productImages

            string filename = Server.MapPath(Url.Content("~/FrontEnd/img/empty_gallery.png"));
            //@"C:\Projekty\SklepInternetowy\SklepInternetowy\FrontEnd\img\empty_gallery.png";
            //string filename = @"E:\Projekty\SklepInternetowy\SklepInternetowy\FrontEnd\img\empty_gallery.png";
            byte[] bytes = System.IO.File.ReadAllBytes(filename);

            IList<Products> products = _productRepo.GetAll();

            foreach (Products productse in products)
            {
                var productImages = new ProductImages(filename, bytes, productse);
                _productImagesRepository.Save(productImages);
            }

            #endregion

            #region users

            string password = "haslo";

            var crypto = new PBKDF2();
            string enryptPass = crypto.Compute(password);

            string Email = "email@email.com";
            string City = "WWA";
            string Address = "Sik 41/12";
            bool IsAdmin = false;
            string Name = "Name";
            string Surname = "Surname";
            string ipAddress = "102.154.12.12";
            string ZipCode = "12-222";
            string Password = enryptPass;
            string PasswordSalt = crypto.Salt;


            var user = new Users(Name, Surname, Email, Password, City, Address, ZipCode, IsAdmin,
                PasswordSalt, ipAddress);


            string pass = "P@ssw0rd";
            string email2 = "cassteam@outlook.com";
            string enryptPass2 = crypto.Compute(pass);
            string password2 = enryptPass2;
            string passwordSalt = crypto.Salt;

            var user2 = new Users(Name, Surname, email2, password2, City, Address, ZipCode, true,
                passwordSalt, ipAddress);

            _userRepo.Save(user);
            _userRepo.Save(user2);

            #endregion

            #region deliveryTypes

            var deliveryType = new DeliveryTypes("Poczta Polska", Convert.ToDecimal(8.99));
            _deliveryRepo.Save(deliveryType);

            #endregion

            #region orders
            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                System.Threading.Thread.Sleep(50);
                var order1 = new Orders(user, DateTime.Now.AddDays(-2.0), Convert.ToDecimal(rnd.Next(50, 400)), deliveryType);
                var order2 = new Orders(user2, DateTime.Now.AddDays(-rnd.Next(1, 50)), Convert.ToDecimal(rnd.Next(50, 400)), deliveryType);
                _ordersRepository.Save(order1);
                _ordersRepository.Save(order2);
            }

            var order = new Orders(user, DateTime.Now, Convert.ToDecimal(29.99), deliveryType);
            _ordersRepository.Save(order);

            #endregion

            
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            FormsAuthentication.SignOut();


            return RedirectToAction("index", "Home");
        }
    }
}