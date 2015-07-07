using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests.ProductsRepositoryTests
{
    [TestClass]
    public class ProductsRepositoryTests
    {
        static INhibernateConnection connection = new NHibernateConnection();
        private IProductsRepository _productsRepository = new ProductsRepository(connection);
        private ICategoryRepository _categoryRepository = new CategoryRepository(connection);
        private IManufacturersRepository _manufacturersRepository = new ManufacturersRepository(connection);

       
        [TestMethod]
        public void SaveProduct()
        {
            var category = _categoryRepository.GetAll().First();
            var manufacturer = _manufacturersRepository.GetAll().First();

            Assert.IsNotNull(category);
            Assert.IsNotNull(manufacturer);

            var product = 
                new Products("TestName", "", Convert.ToDecimal(29.99), category, manufacturer, 1, true, true, true, "");

            Assert.AreEqual(0,product.ID);

            _productsRepository.Save(product);

            Assert.AreNotEqual(0,product.ID);
        }

        [TestMethod]
        public void GetProduct()
        {
            var product = _productsRepository.GetAll().First(x => x.Name == "TestName");

            Assert.IsNotNull(product);

            var product2 = _productsRepository.Get(product.ID);

            Assert.IsNotNull(product2);
        }

        [TestMethod]
        public void GetAllProducts()
        {
            var products = _productsRepository.GetAll();
            Assert.IsNotNull(products);

            if (products.Count >= 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new Exception("Count of product images could not be less than 0");
            }
        }

        [TestMethod]
        public void SearchProduct()
        {
            var product = _productsRepository.GetAll().First(x => x.Name == "TestName");

            Assert.IsNotNull(product);

            int categoryId = product.Category.ID;
            string searchText = "TestName";

            var result = _productsRepository.SearchProducts(categoryId, searchText).First();

            Assert.IsNotNull(result);
            Assert.AreEqual(searchText,result.Name);
            Assert.AreEqual(categoryId,result.Category.ID);
        }

        [TestMethod]
        public void FindProductDuplicateByName()
        {
            string name = "TestName";
            var result = _productsRepository.FindDuplicateByName(name);
            Assert.IsFalse(result);

            name = "NewTestName";
            result = _productsRepository.FindDuplicateByName(name);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdateProduct()
        {
            var product = _productsRepository.GetAll().First(x => x.Name == "TestName");

            Assert.IsNotNull(product);

            string newName = "NewTestName";
            string description = "NewDescription";
            product.Name = newName;
            product.Description = description;

            _productsRepository.Update(product);

            product = _productsRepository.Get(product.ID);

            Assert.IsNotNull(product);
            Assert.AreEqual("NewTestName",product.Name);
            Assert.AreEqual("NewDescription",product.Description);
        }

        [TestMethod]
        public void DeleteProduct()
        {
            var product = _productsRepository.GetAll().First(x => x.Name == "NewTestName");
            int productId = product.ID;
            Assert.IsNotNull(product);

            _productsRepository.Delete(product);

            var product2 = _productsRepository.Get(productId);
            Assert.IsNull(product2);
        }
    }
}
