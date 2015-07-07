using System.Collections.Generic;
using System.Web.UI.WebControls;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly INhibernateConnection _connection;
        private readonly ISession _session;

        public OrderDetailsRepository(INhibernateConnection connection)
        {
            _connection = connection;
            _session = _connection.CreateSessionFactory().OpenSession();
        }

        public IList<OrderDetails> GetAll()
        {
            return _session.QueryOver<OrderDetails>()
                .List();
        }

        public void Save(OrderDetails entity)
        {
            _session.Save(entity);
        }

        public OrderDetails Get(int id)
        {
            return _session.Get<OrderDetails>(id);
        }

        public IList<OrderDetails> GetForOrder(int orderId)
        {
            return _session.QueryOver<OrderDetails>().Where(x => x.Order.Id == orderId).List();
        }

        public void Delete(OrderDetails entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(entity);
                _session.Transaction.Commit();
            }
        }
    }
}