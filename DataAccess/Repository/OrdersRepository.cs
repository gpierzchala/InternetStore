using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;


namespace DataAccess.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly INhibernateConnection _connection;
        private readonly ISession _session;

        public OrdersRepository(INhibernateConnection connection)
        {
            _connection = connection;
            _session = _connection.CreateSessionFactory().OpenSession();
        }


        public Orders Get(int id)
        {
            return _session.Get<Orders>(id);
        }

        public IList<Orders> GetAll()
        {
            return _session.QueryOver<Orders>().List();
        }

        public IList<Orders> GetAll(Guid userId)
        {
            var orders = GetAll();
            var userOrders = orders.Where(x => x.User.ID == userId).ToList();
            return userOrders;
        }

        public void Save(Orders entity)
        {
            var orderState =
                _session.QueryOver<OrderState>().Take(1).List().FirstOrDefault();

            if (entity.State == null)
                entity.State = orderState;

            _session.Save(entity);
        }

        public void Update(Orders entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Update(entity);
                _session.Transaction.Commit();
            }
        }

        public void Delete(Orders entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(entity);
                _session.Transaction.Commit();
            }
        }

        public bool UpdateState(int orderId, int newState)
        {
            var order = _session.Get<Orders>(orderId);

            if (order != null)
            {
                var orderState = _session.QueryOver<OrderState>().Where(x => x.ID == newState).SingleOrDefault();
                using (_session.BeginTransaction())
                {
                    order.State = orderState;
                    _session.Update(order);
                    _session.Transaction.Commit();
                }
                return true;
            }

            return false;
        }

        public IList<Orders> GetUserOrders(string email)
        {
            var orders = GetAll();
            return orders.Where(x => x.User.Email == email).ToList();
        }
    }
}