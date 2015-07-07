using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests.DeliveryTypesRepositoryTests
{
    [TestClass]
    public class DeliveryTypesRepositoryTests
    {
        static INhibernateConnection connection = new NHibernateConnection();
        IDeliveryTypesRepository deliveryTypesRepository = new DeliveryTypesRepository(connection);
        DeliveryTypes deliveryType = new DeliveryTypes("TestDeliveryType", Convert.ToDecimal(29.99));
        DeliveryTypes deliveryType2 = null;


        [TestMethod]
        [Priority(100)]
        public void SaveDeliveryType()
        {
            var deliveryType = deliveryTypesRepository.GetAll().FirstOrDefault(x => x.Name == "TestDeliveryType" || x.Name == "Test");

            if (deliveryType == null)
            {
                deliveryType2 = new DeliveryTypes("TestDeliveryType",Convert.ToDecimal(29.99));
                Assert.AreEqual(0, deliveryType2.ID);

                deliveryTypesRepository.Save(deliveryType2);
                Assert.IsNotNull(deliveryType2);
                Assert.AreNotEqual(0, deliveryType2.ID);
            }
            else
            {
                Assert.IsNotNull(deliveryType);
                deliveryTypesRepository.Save(deliveryType);
                Assert.AreNotEqual(0, deliveryType.ID);
            }
        }

        [TestMethod]
        [Priority(90)]
        public void GetDeliveryType()
        {
            if (deliveryType.ID != 0)
            {
                var delivType = deliveryTypesRepository.Get(deliveryType.ID);
                Assert.IsNotNull(delivType);
            }
            else if (deliveryType2 != null && deliveryType2.ID != 0)
            {
                var delivType = deliveryTypesRepository.Get(deliveryType2.ID);
                Assert.IsNotNull(delivType);
            }
            else
            {
                var delivType = new DeliveryTypes("TestDeliveryType", Convert.ToDecimal(29.99));
                deliveryTypesRepository.Save(delivType);
                var delivType2 = deliveryTypesRepository.Get(delivType.ID);
                Assert.IsNotNull(delivType2);
            }
        }

        [TestMethod]
        [Priority(80)]
        public void GetAllDeliveryTypes()
        {
            var deliveryTypes = deliveryTypesRepository.GetAll();

            if (deliveryTypes.Count >= 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new Exception("Count of delivery types could not be less than 0");
            }
            Assert.IsNotNull(deliveryTypes);
        }
    }
}
