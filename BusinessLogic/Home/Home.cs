using System.Collections.Generic;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;

namespace BusinessLogic.Home
{
    public class Home
    {
        private readonly IProductsRepository _producRepo;
        public Home(IProductsRepository productRepo)
        {
            _producRepo = productRepo;
        }

        public IEnumerable<Products> GetAllProducts()
        {
            return _producRepo.GetAll();
        }
    }
}
