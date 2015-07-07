using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests.ShoppingCartRepositoryTests
{
    [TestClass]
    public class ShoppingCartRepositoryTests
    {
        static INhibernateConnection connection = new NHibernateConnection();
        private readonly IShoppingCartRepository _shoppingCartRepository = new ShoppingCartRepository(connection);
        private readonly IProductsRepository _productsRepository = new ProductsRepository(connection);     
        [TestMethod]
        public void SaveShoppingCart()
        {
            var product = _productsRepository.GetAll().First();
            Assert.IsNotNull(product);
            string myCartId = "TestCartId";
            var shoppingCart = new ShoppingCarts(product, 1, myCartId);

            Assert.AreEqual(0,shoppingCart.ID);

            _shoppingCartRepository.Save(shoppingCart);

            Assert.AreNotEqual(0,shoppingCart.ID);
        }

        [TestMethod]
        public void GetShoppingCart()
        {
            var shoppingCart = _shoppingCartRepository.GetAll().First();
            
            Assert.IsNotNull(shoppingCart);
            int id = shoppingCart.ID;

            shoppingCart = null;

            Assert.IsNull(shoppingCart);

            shoppingCart = _shoppingCartRepository.Get(id);

            Assert.IsNotNull(shoppingCart);

        }

        [TestMethod]
        public void GetAllShoppingCarts()
        {
            var shoppingCarts = _shoppingCartRepository.GetAll();

            Assert.IsNotNull(shoppingCarts);

            if (shoppingCarts.Count >= 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new Exception("Count of shoping carts could not be less than 0");
            }
        }

        [TestMethod]
        public void UpdateShoppingCart()
        {
            var shoppingCart = _shoppingCartRepository
                .GetAll().First(x => x.CartId == "TestCartId");

            Assert.IsNotNull(shoppingCart);

            int newQuantity = 999;
            shoppingCart.Quantity = newQuantity;
            int id = shoppingCart.ID;
            _shoppingCartRepository.Save(shoppingCart);

            shoppingCart = null;

            Assert.IsNull(shoppingCart);

            shoppingCart = _shoppingCartRepository.Get(id);

            Assert.IsNotNull(shoppingCart);
            Assert.AreEqual(newQuantity, shoppingCart.Quantity);
        }

        [TestMethod]
        public void DeleteShoppingCart()
        {
            var shoppingCart = _shoppingCartRepository.GetAll().First(x => x.CartId == "TestCartId");

            Assert.IsNotNull(shoppingCart);

            int id = shoppingCart.ID;

            _shoppingCartRepository.Delete(shoppingCart);

            shoppingCart = _shoppingCartRepository.Get(id);

            Assert.IsNull(shoppingCart);
        }
    }
}
