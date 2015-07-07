using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;

namespace DataAccess.Tests
{
    [TestClass]
    public class ConnectionTests
    {
        [TestMethod]
        public void IsConnectionOpen()
        {
            INhibernateConnection connection = new NHibernateConnection();
            ISession session = connection.CreateSessionFactory().OpenSession();

            string expected = "Open";
            string result = session.Connection.State.ToString();


            Assert.IsNotNull(connection);
            Assert.IsNotNull(session);
            Assert.AreEqual(expected,result);
        }
    }
}
