using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;

namespace DataAccess.Repository
{
    public class DeliveryTypesRepository : IDeliveryTypesRepository
    {
        private readonly ISession _session;

        public DeliveryTypesRepository(INhibernateConnection session)
        {
            _session = session.CreateSessionFactory().OpenSession();
        }

        public DeliveryTypes Get(int id)
        {
            return _session.Get<DeliveryTypes>(id);
        }

        public void Save(DeliveryTypes deliveryType)
        {
            _session.Save(deliveryType);
        }

        public IList<DeliveryTypes> GetAll()
        {
            return _session.QueryOver<DeliveryTypes>()
                .OrderBy(x => x.Name).Asc
                .List();
        }

        public void Update(DeliveryTypes deliveryType)
        {
            using (_session.BeginTransaction())
            {
                _session.Update(deliveryType);
                _session.Transaction.Commit();
            }
        }

        public void Delete(DeliveryTypes deliveryType)
        {
            using (_session.BeginTransaction())
            {
                _session.Delete(deliveryType);
                _session.Transaction.Commit();
            }
        }

        public bool FindDuplicateByName(string name)
        {
            var deliveryTypes = _session.QueryOver<DeliveryTypes>()
                .Where(x => x.Name == name)
                .List();

            if (deliveryTypes.Count == 0)
                return true;
            else
                return false;
        }

        public bool FindDuplicateByNameAndId(string name, int id)
        {
            var deliveryTypes = _session.QueryOver<DeliveryTypes>()
                .Where(x => x.Name == name && x.ID != id)
                .List();

            if (deliveryTypes.Count == 0)
                return true;
            else
                return false;
        }
    }
}