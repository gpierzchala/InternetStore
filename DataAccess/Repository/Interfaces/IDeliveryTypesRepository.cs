using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface IDeliveryTypesRepository
    {
        DeliveryTypes Get(int id);
        void Save(DeliveryTypes deliveryType);
        IList<DeliveryTypes> GetAll();
        void Update(DeliveryTypes deliveryType);
        void Delete(DeliveryTypes deliveryType);
        bool FindDuplicateByName(string name);
        bool FindDuplicateByNameAndId(string name, int id);
    }
}
