using System.Linq;
using System.Web.Mvc;
using DataAccess.Repository.Interfaces;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    public class ManageProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public ManageProductsController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }
        
        /// <summary>
        /// Method returns list of products connected with specific category id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        public ActionResult ConnectedProductsWithCategory(int id)
        {
            var products = _productsRepository.GetAll().Where(x => x.Category.ID == id).ToList();
            return View(products);
        }
    }
}