using System;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCrypto;

namespace DataAccess.Tests.UserRepositoryTests
{
    [TestClass]
    public class UsersRepositoryTests
    {
        static INhibernateConnection connection = new NHibernateConnection();
        private readonly IUserRepository _userRepository = new UserRepository(connection);
        [TestMethod]
        public void SaveUser()
        {
            string password = "haslo";
            var crypto = new PBKDF2();
            string enryptPass = crypto.Compute(password);
            string Email = "test@test.com";
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

            var userGuid = user.ID;

            Assert.IsNotNull(user);
            _userRepository.Save(user);
            Assert.AreNotEqual(userGuid,user.ID);
        }

        [TestMethod]
        public void GetUserByEmail()
        {
            string username =  "test@test.com";
            var user = _userRepository.Get(username);

            Assert.IsNotNull(user);
            Assert.AreEqual(username,user.Email);
        }

        [TestMethod]
        public void GetUserByGuid()
        {
            string userame = "test@test.com";
            Guid userId = new Guid();

            var user = _userRepository.Get(userId);

            Assert.IsNull(user);

            user = _userRepository.Get(userame);

            var user2 = _userRepository.Get(user.ID);

            Assert.IsNotNull(user2);
        }

        [TestMethod]
        public void FindUserDuplicateByEmail()
        {
            string username =  "test@test.com";
            var result = _userRepository.FindDuplicateByEmail(username);

            Assert.IsFalse(result);

            username = "xxx@xxx.xx";
            result = _userRepository.FindDuplicateByEmail(username);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteUser()
        {
            string username = "test@test.com";
            var user = _userRepository.Get(username);

            Assert.IsNotNull(user);
            _userRepository.Delete(user);
            user = _userRepository.Get(username);
            Assert.IsNull(user);
        }
    }
}
