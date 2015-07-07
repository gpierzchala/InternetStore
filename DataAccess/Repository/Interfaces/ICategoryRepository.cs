using System.Collections.Generic;
using DataAccess.Entities;


namespace DataAccess.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        bool FindDuplicateByName(string name);
        bool FindDuplicateByNameAndId(string name, int id);
        Categories Get(int id);

        void Save(Categories entity);

        void Update(Categories entity);

        void Delete(Categories entity);
        IList<Categories> GetAll();
    }
}
