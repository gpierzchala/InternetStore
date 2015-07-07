using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class ManufacturersRepository : IManufacturersRepository
    {
        private readonly INhibernateConnection _connection;
        private readonly ISession _session;

        public ManufacturersRepository(INhibernateConnection connection)
        {
            _connection = connection;
            _session = _connection.CreateSessionFactory().OpenSession();
        }

        public bool FindDuplicateByName(string name)
        {
            var categories = _session.QueryOver<Manufacturers>()
               .Where(x => x.Name == name)
               .List();

            if (categories.Count == 0)
                return true;
            else
                return false;
        }

        public bool FindDuplicateByNameAndId(string name, int id)
        {
            var manufacturers = _session.QueryOver<Manufacturers>()
                   .Where(x => x.Name == name && x.ID != id)
                   .List();

            if (manufacturers.Count == 0)
                return true;
            else
                return false;
        }


        public Manufacturers Get(int id)
        {
            return _session.Get<Manufacturers>(id);
        }

        public void Save(Manufacturers entity)
        {
            _session.Save(entity);
        }

        public void Update(Manufacturers entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Update(entity);
                _session.Transaction.Commit();
            }
        }

        public void Delete(Manufacturers entity)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(entity);
                _session.Transaction.Commit();
            }
        }

        public IList<Manufacturers> GetAll()
        {
            return _session.QueryOver<Manufacturers>()
                .OrderBy(x=>x.Name).Asc
                .List();
        }
    }
}
