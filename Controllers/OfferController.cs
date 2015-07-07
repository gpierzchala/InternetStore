using System.Collections.Generic;
using System.Web.Mvc;
using BusinessLogic.Offer;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using PagedList;
using SklepInternetowy.Helpers;

namespace SklepInternetowy.Controllers
{
    public class OfferController : BaseController
    {
        private readonly Offer offerLogic;
        private IEnumerable<Products> _products = null;
        private IPagedList<Products> _onePageOfProducts = null;

        public OfferController(
            ICategoryRepository catRepo, 
            IProductsRepository productRepo, 
            IManufacturersRepository manufacturersRepository)
            : base(catRepo,manufacturersRepository,productRepo)
        {
            offerLogic = new Offer(_productsRepository);
            ViewBag.Categories = GetCategories();
            ViewBag.Manufacturers = GetManufacturers();
            ViewBag.CategorySelectList = GetCategoryList();

            ViewData["SearchOptions"] = new SelectList(Common.GetSortOptions(), "Id", "Name", 1);
        }

        public ActionResult Index(string sortOrder)
        {
            return View();
        }

        public ActionResult Recent(int? page, int? sortOption)
        {
            _products = offerLogic.GetRecent();

            _onePageOfProducts = Common.SortAndPagin(sortOption, page, null, _products);

            if (Request.IsAjaxRequest())
            { return PartialView("_ProductListing", _onePageOfProducts); }

            return View(_onePageOfProducts);
        }

        public ActionResult BestSellers(int? page, int? sortOption)
        {
            _products = offerLogic.GetBestsellers();
            _onePageOfProducts = Common.SortAndPagin(sortOption, page, null, _products);

            if (Request.IsAjaxRequest())
            { return PartialView("_ProductListing", _onePageOfProducts); }

            return View(_onePageOfProducts);
        }

        public ActionResult Special(int? page, int? sortOption)
        {
            _products = offerLogic.GetSpecial();
            _onePageOfProducts = Common.SortAndPagin(sortOption, page, null, _products);

            if (Request.IsAjaxRequest())
            { return PartialView("_ProductListing", _onePageOfProducts); }

            return View(_onePageOfProducts);
        }
	}
}