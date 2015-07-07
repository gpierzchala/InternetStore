using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests
{
    [TestClass]
    public class CategoryRepositoryTests
    {
        static INhibernateConnection connection = new NHibernateConnection();
        Categories category = new Categories(
                "TestCategory",
                "TestDescription");
        ICategoryRepository categoryRepository = new CategoryRepository(connection);
        private Categories category2 = null;

        [TestMethod]
        [Priority(100)]
        public void SaveCategory()
        {
            var category = categoryRepository.GetAll().FirstOrDefault(x => x.Name == "TestCategory" || x.Name == "Test");

            if (category == null)
            {
                category2 = new Categories(
                 "TestCategory",
                 "TestDescription");
                Assert.AreEqual(0, category2.ID);

                categoryRepository.Save(category2);
                Assert.IsNotNull(category2);
                Assert.AreNotEqual(0, category2.ID);
            }
            else
            {
                Assert.IsNotNull(category);
                categoryRepository.Save(category);
                Assert.AreNotEqual(0, category.ID);  
            }
        }

        [TestMethod]
        [Priority(90)]
        public void GetCategory()
        {
            if (category.ID != 0)
            {
                var cat = categoryRepository.Get(category.ID);
                Assert.IsNotNull(cat);
            }
            else if (category2 != null && category2.ID != 0)
            {
                var cat = categoryRepository.Get(category2.ID);
                Assert.IsNotNull(cat);
            }
            else
            {
                var cat2= new Categories("TestCategory", "TestDescription");
                categoryRepository.Save(cat2);
                var cat = categoryRepository.Get(cat2.ID);
                Assert.IsNotNull(cat);   
            }
        }

        [TestMethod]
        [Priority(80)]
        public void GetAllCategories()
        {
            var categories = categoryRepository.GetAll();

            if (categories.Count >= 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new Exception("Count of categories could not be less than 0");
            }
            Assert.IsNotNull(categories);
        }

        [TestMethod]
        [Priority(70)]
        public void UpdateCategory()
        {
            var category = categoryRepository.GetAll().FirstOrDefault(x => x.Name == "TestCategory");

            if (category != null)
            {
                category.Update("Test", "Description");
                categoryRepository.Update(category);
                Assert.AreEqual("Test", category.Name);
                Assert.AreEqual("Description", category.Description);
            }
            else
            {
                var category2 = new Categories("TestCategory", "");
                categoryRepository.Save(category2);
                category2.Update("Test","Description");
                Assert.AreEqual("Test", category2.Name);
                Assert.AreEqual("Description", category2.Description);
            }
        }

        [TestMethod]
        [Priority(60)]
        public void FindDuplicateByName()
        {
            bool isDuplicated = categoryRepository.FindDuplicateByName("Test2");
            Assert.IsTrue(isDuplicated);

            isDuplicated = categoryRepository.FindDuplicateByName("Test");
            Assert.IsFalse(isDuplicated);
        }

        [TestMethod]
        [Priority(50)]

        public void FindDuplicatesByNameAndID()
        {
            var category = categoryRepository.GetAll().FirstOrDefault(x => x.Name == "Test");
            bool isDuplicated = categoryRepository.FindDuplicateByNameAndId(category.Name, category.ID);
            Assert.IsTrue(isDuplicated);

            isDuplicated = categoryRepository.FindDuplicateByNameAndId(
                category.Name, 999);
            Assert.IsFalse(isDuplicated);

            isDuplicated = categoryRepository.FindDuplicateByNameAndId(
                "xxxx",category.ID);
            Assert.IsTrue(isDuplicated);

            isDuplicated = categoryRepository.FindDuplicateByNameAndId("XXXXX", 1234);
            Assert.IsTrue(isDuplicated);
        }

        [TestMethod]
        [Priority(40)]
        public void DeleteCategory()
        {
            var category = categoryRepository.GetAll().FirstOrDefault(x => x.Name == "Test");

            if (category == null)
            {
                category = new Categories("Test","");
                categoryRepository.Save(category);
            }

            categoryRepository.Delete(category);

            var category2 = categoryRepository.Get(category.ID);

            Assert.IsNull(category2);
        }
    }
}
