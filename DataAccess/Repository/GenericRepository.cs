using System;
using System.Collections.Generic;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly INhibernateConnection _connection;
        private readonly ISessionFactory _sessionFactory;

        public GenericRepository(INhibernateConnection connection)
        {
            _connection = connection;
            _sessionFactory = _connection.CreateSessionFactory();
        }

        public T Get(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }

        public void Save(T value)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.SaveOrUpdate(value);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public void Update(T value)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Update(value);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }

                }
            }
        }

        public void Delete(T value)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.Delete(value);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }

                }
            }
        }

        public IList<T> GetAll()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<T>().List();
            }
        }

        public T Get(Guid id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.Get<T>(id);
            }
        }
    }
}