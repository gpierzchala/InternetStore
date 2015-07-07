using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.Tests.ProductImagesRepositoryTests
{
    [TestClass]
    // ToDo: poprawić testy product image repository
    public class ProductImagesRepositoryTests
    {
        static string filename = @"C:\Projekty\SklepInternetowy\SklepInternetowy\FrontEnd\img\empty_gallery.png";
        byte[] bytes = System.IO.File.ReadAllBytes(filename);
        static INhibernateConnection connection = new NHibernateConnection();
        private IProductImagesRepository productImagesRepository = new ProductImagesRepository(connection);
        private IProductsRepository productsRepository = new ProductsRepository(connection);

        [TestMethod]
        [Priority(100)]
        public void SaveProductImage()
        {
            var product = productsRepository.GetAll().First();
            var productImages = new ProductImages(filename, bytes, product);
            
            productImagesRepository.Save(productImages);
            Assert.IsNotNull(product);
            Assert.IsNotNull(productImages);
            Assert.AreNotEqual(0,productImages.ID);

            productImagesRepository.Delete(productImages);
        }

        [TestMethod]
        [Priority(50)]
        public void GetAllImages()
        {
            var images = productImagesRepository.GetAllImages();
            
            Assert.IsNotNull(images);
            if (images.Count >= 0)
            {
                Assert.IsTrue(true);
            }
            else
            {
                throw new Exception("Count of product images could not be less than 0");
            }
        }
        [TestMethod]
        [Priority(50)]
        public void GetProductImage()
        {
            var sampleImg = productImagesRepository
                .GetAllImages().First();

            Assert.IsNotNull(sampleImg);

            int productId = sampleImg.Product.ID;

            var image = productImagesRepository.GetImage(productId);
            
            Assert.IsNotNull(image);
        }

        [TestMethod]
        [Priority(50)]
        public void GetProductWithoutImage()
        {
            var image = productImagesRepository.GetImage(123456789);
            Assert.IsNull(image);
        }

        [TestMethod]
        [Priority(30)]
        public void UpdateProductWithImage()
        {
            string filename = @"C:\Projekty\SklepInternetowy\SklepInternetowy\FrontEnd\img\close.png";
            byte[] newbytes = System.IO.File.ReadAllBytes(filename);

            var productImage = productImagesRepository.GetAllImages().First();
            var product = productImage.Product;
            Assert.IsNotNull(productImage);
            Assert.IsNotNull(product);

            var productOldBytes = productImage.ImageBytes;

            var newProductImage = new ProductImages(filename, newbytes, product);
            productImagesRepository.Update(newProductImage);
            
            Assert.AreNotEqual(productOldBytes,newbytes);

            var getUpdate = productImagesRepository.Get(newProductImage.ID);
            Assert.IsNotNull(getUpdate);
            Assert.AreEqual(newbytes,getUpdate.ImageBytes);
        }

        [TestMethod]
        [Priority(10)]
        public void DeleteProductImage()
        {
            var product = productsRepository.GetAll().First();
            var productImages = new ProductImages(filename, bytes, product);

            productImagesRepository.Save(productImages);

            Assert.IsNotNull(product);
            Assert.IsNotNull(productImages);
            Assert.AreNotEqual(0,productImages.ID);

            productImagesRepository.Delete(productImages);

            var fromDb = productImagesRepository.Get(product.ID);

            Assert.IsNull(fromDb);
        }
    }
}
