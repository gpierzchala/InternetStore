using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests.OrdersRepositoryTests
{
    [TestClass]
    public class OrdersRepositoryTests
    {
        private static INhibernateConnection connection = new NHibernateConnection();
        private readonly IOrdersRepository _ordersRepository = new OrdersRepository(connection);
        private readonly IUserRepository _userRepository = new UserRepository(connection);

        private readonly IDeliveryTypesRepository
            _deliveryTypesRepository = new DeliveryTypesRepository(connection);

        private readonly IOrderDetailsRepository _orderDetailsRepository = new OrderDetailsRepository(connection);

        private readonly IOrderStateRepository _orderStateRepository = new OrderStateRepository(connection);

        [TestMethod]
        public void SaveOrder()
        {
            var user = _userRepository.Get("email@email.com");
            var deliveryType = _deliveryTypesRepository.GetAll().First();
            Assert.IsNotNull(user);

            var order = new Orders(user, DateTime.Now, Convert.ToDecimal(29.99), deliveryType,
                _orderStateRepository.GetAll().FirstOrDefault());
            Assert.AreEqual(0, order.Id);

            _ordersRepository.Save(order);
            Assert.AreNotEqual(0, order.Id);
        }

        [TestMethod]
        public void GetOrder()
        {
            var order = _ordersRepository.GetAll().First();

            Assert.IsNotNull(order);

            int id = order.Id;

            var order2 = _ordersRepository.Get(id);

            Assert.AreEqual(order, order2);
        }

        [TestMethod]
        public void GetAllOrders()
        {
            var orders = _ordersRepository.GetAll();
            Assert.IsNotNull(orders);

            if (orders.Count >= 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new Exception("Count of orders could not be less than 0");
            }
        }

        [TestMethod]
        public void UpdateOrder()
        {
            var orders = _ordersRepository.GetAll();
            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                var orderDetails = _orderDetailsRepository.GetForOrder(order.Id);

                if (orderDetails == null)
                {
                    Assert.IsNotNull(order);
                    int id = order.Id;
                    decimal price = Convert.ToDecimal(999.99);
                    order.SummaryPrice = Convert.ToDecimal(999.99);

                    _ordersRepository.Update(order);

                    order = _ordersRepository.Get(id);

                    Assert.IsNotNull(order);
                    Assert.AreEqual(price, order.SummaryPrice);
                    break;
                }
            }
        }

        [TestMethod]
        public void DeleteOrder()
        {
            var orders = _ordersRepository.GetAll();

            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                var orderDetails = _orderDetailsRepository.GetForOrder(order.Id);
                if (orderDetails == null)
                {
                    _ordersRepository.Delete(order);
                    Assert.IsNull(order);
                    break;
                }
            }
        }
    }
}