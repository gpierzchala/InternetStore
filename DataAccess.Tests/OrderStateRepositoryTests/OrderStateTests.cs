using System.Collections.Generic;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests.OrderStatusRepositoryTests
{
    [TestClass]
    public class OrderStateTests
    {
        static readonly INhibernateConnection connection = new NHibernateConnection();
        private readonly IOrderStateRepository _orderStateRepository = new OrderStateRepository(connection);
        
        [TestMethod]
        public void SaveOrderStatus()
        {
            var orderState = new OrderState("New test status name");
            Assert.IsNotNull(orderState);
            Assert.AreEqual(0, orderState.ID);
            Assert.AreEqual("New test status name", orderState.StateName);

            _orderStateRepository.Save(orderState);
            Assert.AreNotEqual(0, orderState.ID);
        }

        [TestMethod]
        public void GetAllOrderStates()
        {
            var ordeStates = new List<OrderState>();
            Assert.AreEqual(0,ordeStates.Count);

            ordeStates = _orderStateRepository.GetAll().ToList();
            Assert.AreNotEqual(0,ordeStates.Count);
        }

        [TestMethod]
        public void GetOrderStatus()
        {
            var orderState = _orderStateRepository.GetAll();
            Assert.IsNotNull(orderState);

            var singleState = orderState.FirstOrDefault(x => x.StateName == "New test status name");
            Assert.IsNotNull(singleState);
            Assert.AreEqual("New test status name",singleState.StateName);
        }

        [TestMethod]
        public void DeleteOrderStatus()
        {
            var orderState = _orderStateRepository.GetAll();
            Assert.IsNotNull(orderState);

            var singleState = orderState.FirstOrDefault(x => x.StateName == "New test status name");
            Assert.IsNotNull(singleState);

            var stateId = singleState.ID;

            _orderStateRepository.Delete(singleState);

            var stateShouldBeNull = _orderStateRepository.Get(stateId);
            Assert.IsNull(stateShouldBeNull);
        }
    
    }
}
