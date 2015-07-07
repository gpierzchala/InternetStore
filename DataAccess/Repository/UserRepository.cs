using System;
using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly INhibernateConnection _connection;
        private readonly ISession _session;

        public UserRepository(INhibernateConnection connection)
        {
            _connection = connection;
            _session = _connection.CreateSessionFactory().OpenSession();
        }

        public IList<Users> GetAllUsers()
        {
            return _session.QueryOver<Users>().List();
        }

        public void Save(Users entity)
        {
            using (ISession session = _connection.CreateSessionFactory().OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(entity);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public Users Get(Guid id)
        {
            return _session.Get<Users>(id);
        }

        public Users Get(string email)
        {
            return _session.QueryOver<Users>().Where(x => x.Email == email).SingleOrDefault();
        }

        public bool FindDuplicateByEmail(string email)
        {
            IList<Users> users = _session.QueryOver<Users>()
                .Where(x => x.Email == email)
                .List();

            if (users.Count == 0)
                return true;
            return false;
        }

        public void Delete(Users user)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(user);
                _session.Transaction.Commit();
            }
        }

        public void Update(string userLogin,string name, string surname, string email, string city, string street, string zipCode)
        {
            var user = Get(userLogin);

            if (user != null)
            {
                user.Name = name;
                user.Surname = surname;
                user.Email = email;
                user.City = city;
                user.Address = street;
                user.ZipCode = zipCode;

                try
                {
                    using (_session.BeginTransaction())
                    {
                        _session.Update(user);
                        _session.Transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Pojawił się problem z aktualizacją danych. Jeżeli problem będzie się powatarzał skontaktuj się z nami.");
                }
            }
        }

        public void ChangeUserRole(string email)
        {
            var user = _session.QueryOver<Users>().Where(x => x.Email == email).SingleOrDefault();

            if (user != null)
            {
                using (_session.BeginTransaction())
                {
                    user.IsAdmin = user.IsAdmin != true;
                     
                    _session.Update(user);
                    _session.Transaction.Commit();
                }
            }
        }
    }
}