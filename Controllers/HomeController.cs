using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLogic.Home;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using PagedList;
using SklepInternetowy.Helpers;

namespace SklepInternetowy.Controllers
{
    public class HomeController : BaseController
    {
        private IEnumerable<Products> _products = null;
        private IEnumerable<Products> _specificProducts = null;
        private IPagedList<Products> _onePageOfProducts = null;

        private readonly Home homeLogic;
        public HomeController(
            ICategoryRepository catRepo,
            IProductsRepository productRepo,
            IManufacturersRepository manufacturerRepo) 
            : base(catRepo,manufacturerRepo,productRepo)
        {
            homeLogic = new Home(_productsRepository);
            ViewBag.Categories = GetCategories();
            ViewBag.Manufacturers = GetManufacturers();
            ViewBag.CategorySelectList = GetCategoryList();
        }

        public ActionResult Index(int? featuredPage, int? recentPage, int? page, int? sortOption, string search,
            string category)
        {
            _products = homeLogic.GetAllProducts();
            _specificProducts = _products.Where(x => x.IsFeatured);
            _onePageOfProducts = Common.SortAndPagin(sortOption, page, 4, _specificProducts);
            ViewData["featured"] = _onePageOfProducts;

            _specificProducts = _products.Where(x => x.IsRecent);
            _onePageOfProducts = Common.SortAndPagin(sortOption, page, 4, _specificProducts);
            ViewData["recent"] = _onePageOfProducts;
            
            return View();
        }

        public ActionResult Recent(int recentPage)
        {
            _products = homeLogic.GetAllProducts();
            _specificProducts = _products.Where(x => x.IsRecent);
            _onePageOfProducts = Common.SortAndPagin(1, recentPage, 4, _specificProducts);

            if(Request.IsAjaxRequest())
            { return PartialView("_RecentProducts", _onePageOfProducts); }

            return View("Index");
        }

        public ActionResult Featured(int featuredPage)
        {
            _products = homeLogic.GetAllProducts();
            _specificProducts = _products.Where(x => x.IsFeatured);
            _onePageOfProducts = Common.SortAndPagin(1, featuredPage, 4, _specificProducts);
            if(Request.IsAjaxRequest())
            { return PartialView("_FeaturedProducts", _onePageOfProducts); }

            return View("Index");
        }
    }
}