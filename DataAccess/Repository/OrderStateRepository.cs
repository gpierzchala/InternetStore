using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class OrderStateRepository : IOrderStateRepository
    {
        private readonly ISession _session;
        public OrderStateRepository(INhibernateConnection connection)
        {
            _session = connection.CreateSessionFactory().OpenSession();
        }
        public OrderState Get(int id)
        {
            return _session.Get<OrderState>(id);
        }

        public void Save(OrderState newOrderState)
        {
            using (_session.BeginTransaction())
            {
                _session.Save(newOrderState);
                _session.Transaction.Commit();
            }
        }

        public IList<OrderState> GetAll()
        {
            return _session.QueryOver<OrderState>().List();
        }

        public void Delete(OrderState orderState)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(orderState);
                _session.Transaction.Commit();
            }
        }
    }
}
