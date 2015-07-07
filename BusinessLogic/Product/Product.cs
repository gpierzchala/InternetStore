using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;

namespace BusinessLogic.Product
{
    public class Product
    {
        private readonly IProductsRepository _productRepo;

        public Product(IProductsRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public Products GetProduct(int id)
        {
            return _productRepo.Get(id);
        }

        public IEnumerable<Products> GetSimilarProducts(int count, string category, int productId)
        {
            return _productRepo.GetAll()
                .Where(x => x.Category.Name == category)
                .Where(x => x.ID != productId)
                .OrderBy(x => new Random().Next())
                .Take(count);
        }

        public IEnumerable<Products> GetProductsInCategory(int categoryId)
        {
            return _productRepo.GetAll().Where(x => x.Category.ID == categoryId);
        }

        public IEnumerable<Products> GetAllProducts()
        {
            return _productRepo.GetAll().OrderBy(x => x.Name);
        }

        public IEnumerable<Products> GetManufacturerProducts(int manufacturerId)
        {
            return _productRepo.GetAll().Where(x => x.Manufacturer.ID == manufacturerId);
        }
    }
}