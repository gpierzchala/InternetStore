using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ISession _session;
        public CategoryRepository(INhibernateConnection connection)
        {
            _session = connection.CreateSessionFactory().OpenSession();
        }

        public bool FindDuplicateByName(string name)
        {
            var categories = _session.QueryOver<Categories>()
                .Where(x => x.Name == name)
                .List();

            if (categories.Count == 0)
                return true;
            else
                return false;
        }

        public bool FindDuplicateByNameAndId(string name, int id)
        {
             var categories = _session.QueryOver<Categories>()
                   .Where(x => x.Name == name && x.ID != id)
                   .List();

            if (categories.Count == 0)
                return true;
            else
                return false;
        }

        public Categories Get(int id)
        {
            return _session.Get<Categories>(id);
        }

        public void Save(Categories entity)
        {
            _session.Save(entity);
        }

        public void Update(Categories entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Update(entity);
                _session.Transaction.Commit();
            }
        }

        public void Delete(Categories entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(entity);
                _session.Transaction.Commit();
            }
        }

        public IList<Categories> GetAll()
        {
            return _session.QueryOver<Categories>()
                .OrderBy(x=>x.Name).Asc
                .List();
        }
    }
}
