using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCrypto;
using SklepInternetowy.Helpers;
using SklepInternetowy.Models;

namespace SklepInternetowy.Tests
{
    [TestClass]
    public class DataAccessTests
    {
        [TestMethod]
        public void TestMethod()
        {
            var conn = new NHibernateConnection();
            var categories = new GenericRepository<Categories>(conn);
            List<Categories> list = categories.GetAll().ToList();
            int count = list.Count;
            //Test t = new Test
            //{
            //    LastName = "LastName",
            //    Name = "Name"
            //};

            //GenericRepository<Test> tRepo = new GenericRepository<Test>();
            //tRepo.Save(t);
        }

        [TestMethod]
        public void Manufacturers()
        {
            var manufacturer = new Manufacturers("Mój producent");
            var conn = new NHibernateConnection();
            var repo = new ManufacturersRepository(conn);

            //repo.Save(manufacturer);

            Manufacturers fromDB = repo.Get(2);

            //fromDB.Update("Nowa nazwa");

            //repo.Update(fromDB);
            //Assert.IsFalse(fromDB.Name != "Nowa nazwa");
            repo.Delete(fromDB);

            Manufacturers manufacturrer2 = repo.Get(2);
            Assert.IsNull(manufacturrer2);
        }

        [TestMethod]
        public void Products()
        {
            var conn = new NHibernateConnection();
            var productRepo = new ProductsRepository(conn);
            var categoryRepo = new CategoryRepository(conn);
            var manufacturersRepository = new ManufacturersRepository(conn);
            Categories category = categoryRepo.Get(4);

            Manufacturers manufacturer = manufacturersRepository.Get(1);

            var product = new Products("Towar", "Jego opis", Convert.ToDecimal(125.99), category, manufacturer, 100,
                false, true, false, "Krótki opis");


            productRepo.Save(product);

            Products fromDB = productRepo.Get(product.ID);

            Assert.AreEqual(product, fromDB);
        }

        [TestMethod]
        public void USers()
        {
            var conn = new NHibernateConnection();
            var userRepo = new UserRepository(conn);
            string password = "haslo";

            var crypto = new PBKDF2();
            string enryptPass = crypto.Compute(password);


            string Email = "email@email.com";
            string City = "WWA";
            string FlatNumber = "1";
            string HouseNumber = "5";
            bool IsAdmin = false;
            string Name = "Name";
            string Surname = "Surname";
            string Street = "Street";
            string ZipCode = "12-222";
            string Password = enryptPass;
            string PasswordSalt = crypto.Salt;

            //var user = new Users(Name, Surname, Email, Password, City, Street, HouseNumber, FlatNumber, ZipCode, IsAdmin,
            //    PasswordSalt);

            //userRepo.Save(user);

            Users fromDb = userRepo.Get(Email);
            //Assert.AreEqual(user,fromDb);

            bool isValid = true;

            var login = new LoginModel
            {
                Email = "email@email.com",
                Password = "haslo"
            };

            bool result = CryptoHelpers.IsValid(fromDb, login.Password);

            Assert.AreEqual(isValid, result);
        }

        [TestMethod]
        public void ShoppingCart()
        {
            var conn = new NHibernateConnection();
            var repo = new ShoppingCartRepository(conn);
            var producRepo = new ProductsRepository(conn);
            IList<Products> products = producRepo.GetAll();

            Products product1 = products.First();
            IEnumerable<Products> product2 = products.Where(x => x.IsRecent).Take(1);

            var shoppingCart = new ShoppingCarts(product1, 1, "12345678", DateTime.Now);

            repo.Save(shoppingCart);

            ShoppingCarts fromDB = repo.GetAll().Where(x => x.CartId == "12345678").FirstOrDefault();

            Assert.IsNotNull(fromDB);
        }


        [TestMethod]
        public void Orders()
        {
            var conn = new NHibernateConnection();
            var repo = new OrdersRepository(conn);
            var productRepo = new ProductsRepository(conn);
            var userRepo = new UserRepository(conn);
            Users user = userRepo.Get("cassteam@outlook.com");
            var genericRepo = new GenericRepository<Orders>(conn);


            //var order = new Orders(user, DateTime.Now,
            //    Convert.ToDecimal(299.99));

            //repo.Save(order);

            var fromDB = repo.Get(53);
            Assert.IsNotNull(fromDB);
        }

        [TestMethod]
        public void OrderDetails()
        {
            var conn = new NHibernateConnection();
            var orderRepo = new OrdersRepository(conn);
            var productRepo = new ProductsRepository(conn);
            var userRepo = new UserRepository(conn);
            var deliveryRepo = new DeliveryTypesRepository(conn);
            Users user = userRepo.Get("cassteam@outlook.com");
            var orderDetailsRepo = new OrderDetailsRepository(conn);
            Products product = productRepo.GetAll().First();

            var delivetyType = deliveryRepo.GetAll().First();

            if (delivetyType == null)
            {
                delivetyType = new DeliveryTypes("Poczta Polska", Convert.ToDecimal(8.99));
                deliveryRepo.Save(delivetyType);
            }
            

            var order = new Orders(user, DateTime.Now,
                Convert.ToDecimal(299.99),delivetyType);

            orderRepo.Save(order);

            var orderDetails = new OrderDetails(order, product, 2, Convert.ToDecimal(29.99));
            orderDetailsRepo.Save(orderDetails);

            var fromDB = orderDetailsRepo.Get(orderDetails.ID);

            Assert.IsNotNull(fromDB);
        }

        [TestMethod]
        public void DeliveryTypes()
        {
            var conn = new NHibernateConnection();
            var deliveryRepo = new DeliveryTypesRepository(conn);

            DeliveryTypes newType = new DeliveryTypes("Poczta polska",Convert.ToDecimal(7.99));

            deliveryRepo.Save(newType);

            var fromDB = deliveryRepo.Get(newType.ID);
            Assert.IsNotNull(fromDB);
        }
    }
}