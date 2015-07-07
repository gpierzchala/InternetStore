using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface IManufacturersRepository
    {
        bool FindDuplicateByName(string name);
        bool FindDuplicateByNameAndId(string name,int id);
        Manufacturers Get(int id);

        void Save(Manufacturers entity);

        void Update(Manufacturers entity);

        void Delete(Manufacturers entity);
        IList<Manufacturers> GetAll();
    }
}
