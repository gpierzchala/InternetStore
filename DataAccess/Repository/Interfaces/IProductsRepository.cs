using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface IProductsRepository
    {
        bool FindDuplicateByName(string name);

        bool FindDuplicateByNameAndID(string name, int id);
        Products Get(int id);

        void Save(Products entity);

        void Update(Products entity);

        void Delete(Products entity);
        IList<Products> GetAll();
        IList<Products> SearchProducts(int? categoryId, string text);
        IList<Products> GetByCategory(int categoryId);
        IList<Products> GetByManufacturer(int manufacturerId);
    }
}