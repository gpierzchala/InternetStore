using System;
using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class ProductImagesRepository : IProductImagesRepository
    {
         private readonly ISession _session;
         public ProductImagesRepository(INhibernateConnection connection)
        {
            _session = connection.CreateSessionFactory().OpenSession();
        }

        public IList<ProductImages> GetAllImages()
        {
            return _session.QueryOver<ProductImages>()
                .List();
        }

        public ProductImages GetImage(int productId)
        {
            return _session.QueryOver<ProductImages>()
                .Where(x => x.Product.ID == productId)
                .Take(1).SingleOrDefault();
        }

        public void Update(ProductImages entity)
        {
            using (_session.BeginTransaction())
            {
                try
                {
                    _session.SaveOrUpdate(entity);
                    _session.Transaction.Commit();
                }
                catch (Exception)
                {
                   _session.Transaction.Rollback();
                }
            }
        }

        public void Delete(ProductImages entity)
        {
            using (_session.BeginTransaction())
            {
                try
                {
                    _session.Delete(entity);
                    _session.Transaction.Commit();
                }
                catch (Exception)
                {
                    _session.Transaction.Rollback();
                }
            }
        }

        public void Save(ProductImages entity)
        {
            _session.Save(entity);
        }

        public ProductImages Get(int id)
        {
            return _session.Get<ProductImages>(id);
        }
    }
}
