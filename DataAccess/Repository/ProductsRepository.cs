using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;
using NHibernate.Criterion;


namespace DataAccess.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly INhibernateConnection _connection;
        private readonly ISession _session;
        public ProductsRepository(INhibernateConnection connection)
        {
            _connection = connection;
            _session = _connection.CreateSessionFactory().OpenSession();
        }

        public bool FindDuplicateByName(string name)
        {
            var products = _session.QueryOver<Products>()
                   .Where(x => x.Name == name)
                   .List();

            if (products.Count == 0)
                return true;
            else
                return false;
        }

        public bool FindDuplicateByNameAndID(string name, int id)
        {
            var products = _session.QueryOver<Products>()
                .Where(x => x.Name == name && x.ID != id)
                .List();

            if (products.Count == 0)
                return true;
            else
                return false;
        }

        public Products Get(int id)
        {
            return _session.Get<Products>(id);
        }

        public void Save(Products entity)
        {
            _session.Save(entity);
        }

        public void Update(Products entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Update(entity);
                _session.Transaction.Commit();
            }
        }

        public void Delete(Products entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(entity);
                _session.Transaction.Commit();
            }
        }

        public IList<Products> GetAll()
        {
            return _session.QueryOver<Products>()
                .OrderBy(x=>x.Name).Asc
                .List();
        }

        public IList<Products> SearchProducts(int? categoryId, string text)
        {
            if (categoryId == null)
            {
                return _session.QueryOver<Products>()
               .WhereRestrictionOn(x => x.Name).IsInsensitiveLike(text, MatchMode.Anywhere)
               .List();
            }

           return _session.QueryOver<Products>()
               .Where(x=>x.Category.ID == categoryId.Value)
               .WhereRestrictionOn(x=>x.Name).IsInsensitiveLike(text,MatchMode.Anywhere)
               .List();
        }

        public IList<Products> GetByCategory(int categoryId)
        {
            return _session.QueryOver<Products>().Where(x => x.Category.ID == categoryId).List();
        }

        public IList<Products> GetByManufacturer(int manufacturerId)
        {
            return _session.QueryOver<Products>().Where(x => x.Manufacturer.ID == manufacturerId).List();
        }
    }
}
