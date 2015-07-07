using System.Collections.Generic;
using System.Web.Mvc;
using BusinessLogic.Product;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using PagedList;
using SklepInternetowy.Helpers;

namespace SklepInternetowy.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly Product _productLogic;
        private IEnumerable<Products> _products = null;
        private IPagedList<Products> _onePageOfProducts = null;

        public ProductController(
            IProductImagesRepository productImageRepo,
            IProductsRepository productsRepo,
            ICategoryRepository catRepo,
            IManufacturersRepository manufacturersRepository) 
            : base(catRepo,manufacturersRepository,productsRepo)
        {
            _productLogic = new Product(productsRepo);
            _productImagesRepository = productImageRepo;
            ViewBag.Categories = GetCategories();
            ViewBag.Manufacturers = GetManufacturers();
            ViewBag.CategorySelectList = GetCategoryList();
            ViewData["SearchOptions"] = new SelectList(Common.GetSortOptions(), "Id", "Name", 1);
        }

        public ActionResult Details(int id)
        {
            var product = _productLogic.GetProduct(id);
            if (product == null)
                return View("_Error");

            _products = _productLogic.GetSimilarProducts(6, product.Category.Name, product.ID);
            _onePageOfProducts = Common.SortAndPagin(null, 1, 6, _products);
            ViewData["similarProducts"] = _onePageOfProducts;

            return View(product);
        }


        public ActionResult Category(int id, int? page, int? sortOption)
        {
            _products = _productLogic.GetProductsInCategory(id);

            if (_products == null)
                return View(("_Error"));

            ViewBag.CategoryName = CatRepo.Get(id).Name;
            _onePageOfProducts = Common.SortAndPagin(sortOption, page, null, _products);

            if (Request.IsAjaxRequest())
                return PartialView("_ProductListing", _onePageOfProducts);

            return View(_onePageOfProducts);
        }

        public ActionResult Manufacturer(int id, int? page, int? sortOption)
        {
            _products = _productLogic.GetManufacturerProducts(id);

            if (_products == null)
                return View(("_Error"));

            ViewBag.ManufacturerName = ManufacturersRepository.Get(id).Name;
            _onePageOfProducts = Common.SortAndPagin(sortOption, page, null, _products);

            if (Request.IsAjaxRequest())
                return PartialView("_ProductListing", _onePageOfProducts);
            

            return View(_onePageOfProducts);
        }

        public ActionResult Products(int? page, int? sortOption)
        {
            _products = _productLogic.GetAllProducts();

            if (_products == null)
                return View(("_Error"));

            _onePageOfProducts = Common.SortAndPagin(sortOption, page, null, _products);
            if (Request.IsAjaxRequest())
                return PartialView("_ProductListing", _onePageOfProducts);
            

            return View(_onePageOfProducts);
        }

        public ActionResult Search(int? page, int? sortOption, string search, int? category)
        {
            _products = ProductsRepository.SearchProducts(category, search);

            if (_products == null)
                return View(("_Error"));

            _onePageOfProducts = Common.SortAndPagin(sortOption, page, null, _products);
            ViewBag.Search = search;

            if (Request.IsAjaxRequest())
                return PartialView("_ProductListing", _onePageOfProducts);
            
            return View("Search", _onePageOfProducts);
        }

        public FileContentResult ViewImage(int productID)
        {
            ProductImages image = _productImagesRepository.GetImage(productID);
            if (image == null)
            {
                string path = Server.MapPath(Url.Content("~/FrontEnd/img/empty_gallery.png"));
                    //@"C:\Projekty\SklepInternetowy\SklepInternetowy\FrontEnd\img\empty_gallery.png";
                byte[] buffer = System.IO.File.ReadAllBytes(path);
                return new FileContentResult(buffer, "image/jpg");
            }
            else
            {
                byte[] buffer = image.ImageBytes;
                return new FileContentResult(buffer, "image/jpg");
            }
        }
    }
}