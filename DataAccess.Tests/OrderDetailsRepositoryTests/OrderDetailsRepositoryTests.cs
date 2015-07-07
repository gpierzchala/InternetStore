using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests.OrderDetailsRepositoryTests
{
    [TestClass]
    public class OrderDetailsRepositoryTests
    {
        static INhibernateConnection connection = new NHibernateConnection();
        private readonly IOrderDetailsRepository _orderDetailsRepository = new OrderDetailsRepository(connection);
        private readonly IOrdersRepository _ordersRepository = new OrdersRepository(connection);
        private readonly IUserRepository _userRepository = new UserRepository(connection);
        private readonly IDeliveryTypesRepository
            _deliveryTypesRepository = new DeliveryTypesRepository(connection);
        private readonly IOrderStateRepository _orderStateRepository = new OrderStateRepository(connection);
        private readonly IProductsRepository _productsRepository = new ProductsRepository(connection);
        [TestMethod]
        public void SaveOrderDetails()
        {
            var user = _userRepository.Get("email@email.com");
            var deliveryType = _deliveryTypesRepository.GetAll().First();
            var product = _productsRepository.GetAll().First();

            Assert.IsNotNull(product);
            Assert.IsNotNull(user);

            var order = new Orders(user,DateTime.Now,Convert.ToDecimal(98765),deliveryType, _orderStateRepository.GetAll().First());

            _ordersRepository.Save(order);

            Assert.AreNotEqual(0,order.Id);

            var orderDetails = new OrderDetails(order, product, 1, Convert.ToDecimal(98765));

            Assert.AreEqual(0,orderDetails.ID);

            _orderDetailsRepository.Save(orderDetails);

            Assert.AreNotEqual(0,orderDetails.ID);
        }

        [TestMethod]
        public void GetOrderDetails()
        {
            var orderDetails = _orderDetailsRepository.GetAll().First();
            Assert.IsNotNull(orderDetails);
            int id = orderDetails.ID;
            orderDetails = null;
            Assert.IsNull(orderDetails);
            orderDetails = _orderDetailsRepository.Get(id);
            Assert.IsNotNull(orderDetails);
        }

        [TestMethod]
        public void GetAllOrderDetails()
        {
            var orderDetails = _orderDetailsRepository.GetAll();

            Assert.IsNotNull(orderDetails);

            if (orderDetails.Count >= 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new Exception("Count of orders details could not be less than 0");
            }
        }

        [TestMethod]
        public void DeleteOrderDetails()
        {
            var orderDetails = _orderDetailsRepository
                .GetAll().First(x => x.UnitPrice == Convert.ToDecimal(98765));
            
            Assert.IsNotNull(orderDetails);

            var order = _ordersRepository.GetAll().First(x => x.SummaryPrice == Convert.ToDecimal(98765));

            Assert.IsNotNull(order);

            int id = orderDetails.ID;
            _orderDetailsRepository.Delete(orderDetails);
            _ordersRepository.Delete(order);
            orderDetails = _orderDetailsRepository.Get(id);

            Assert.IsNull(orderDetails);
        }
    }
}
