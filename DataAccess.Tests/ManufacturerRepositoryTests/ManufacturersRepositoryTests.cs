using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests.ManufacturerRepositoryTests
{
    [TestClass]
    public class ManufacturersRepositoryTests
    {
        static INhibernateConnection connection = new NHibernateConnection();
        IManufacturersRepository manufacturersRepository = new ManufacturersRepository(connection);
        Manufacturers manufacturer = new Manufacturers("TestManufacturer");
        Manufacturers manufacturer2 = null;

        [TestMethod]
        [Priority(100)]
        public void SaveManufacturer()
        {
            var manufacturer = manufacturersRepository.GetAll().FirstOrDefault(x => x.Name == "TestManufacturer" || x.Name == "Test");

            if (manufacturer == null)
            {
                manufacturer2 = new Manufacturers("TestDeliveryType");
                Assert.AreEqual(0, manufacturer2.ID);

                manufacturersRepository.Save(manufacturer2);
                Assert.IsNotNull(manufacturer2);
                Assert.AreNotEqual(0, manufacturer2.ID);
            }
            else
            {
                Assert.IsNotNull(manufacturer);
                manufacturersRepository.Save(manufacturer);
                Assert.AreNotEqual(0, manufacturer.ID);
            }
        }

        [TestMethod]
        [Priority(90)]
        public void GetManufacturer()
        {
            if (manufacturer.ID != 0)
            {
                var manufact = manufacturersRepository.Get(manufacturer.ID);
                Assert.IsNotNull(manufact);
            }
            else if (manufacturer2 != null && manufacturer2.ID != 0)
            {
                var manufact = manufacturersRepository.Get(manufacturer2.ID);
                Assert.IsNotNull(manufact);
            }
            else
            {
                var manufact = new Manufacturers("TestDeliveryType");
                manufacturersRepository.Save(manufact);
                var manufact2 = manufacturersRepository.Get(manufact.ID);
                Assert.IsNotNull(manufact2);
            }
        }

        [TestMethod]
        [Priority(80)]
        public void GetAllManufacturers()
        {
            var manufacturers = manufacturersRepository.GetAll();

            if (manufacturers.Count >= 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new Exception("Count of manufacturers could not be less than 0");
            }

            Assert.IsNotNull(manufacturers);
        }
    }
}
