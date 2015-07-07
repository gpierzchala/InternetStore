using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface IProductImagesRepository
    {
        IList<ProductImages> GetAllImages();
        ProductImages GetImage(int productId);
        void Update(ProductImages entity);
        void Save(ProductImages entity);
        void Delete(ProductImages entity);
        ProductImages Get(int id);
    }
}
