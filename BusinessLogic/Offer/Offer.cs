using System.Collections.Generic;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;

namespace BusinessLogic.Offer
{
    public class Offer
    {
        private readonly IProductsRepository _productRepo;

        public Offer(IProductsRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public IEnumerable<Products> GetBestsellers()
        {
            return _productRepo.GetAll().Where(x => x.IsBestSeller);
        }

        public IEnumerable<Products> GetRecent()
        {
            return _productRepo.GetAll().Where(x => x.IsRecent);
        }

        public IEnumerable<Products> GetSpecial()
        {
            return _productRepo.GetAll().Where(x => x.IsFeatured);
        }
    }
}