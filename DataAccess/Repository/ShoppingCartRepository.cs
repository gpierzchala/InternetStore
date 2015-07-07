using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ISession _session;
        public ShoppingCartRepository(INhibernateConnection connection)
        {
            _session = connection.CreateSessionFactory().OpenSession();
        }

        public ShoppingCarts Get(int id)
        {
            return _session.Get<ShoppingCarts>(id);
        }

        public IList<ShoppingCarts> GetAll()
        {
            return _session.QueryOver<ShoppingCarts>()
                .List();
        }

        public void Save(ShoppingCarts entity)
        {
            _session.Save(entity);
        }

        public void Update(ShoppingCarts cartItem)
        {
            using (_session.BeginTransaction())
            {
                _session.Update(cartItem);
                _session.Transaction.Commit();
            }
        }

        public void Delete(ShoppingCarts cartItem)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(cartItem);
                _session.Transaction.Commit();
            }
        }
    }
}
