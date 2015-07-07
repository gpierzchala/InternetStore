using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;
using SklepInternetowy.Areas.Admin.Models;
using SklepInternetowy.Helpers;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IManufacturersRepository _manufacturersRepo;
        private readonly IProductImagesRepository _productImagesRepository;
        private readonly IProductsRepository _productRepo;
        private readonly ISession _session;


        public ProductsController(
            INhibernateConnection connection, IProductsRepository repo,
            ICategoryRepository categoryRepo,
            IManufacturersRepository manufacturersRepo,
            IProductImagesRepository productImageRepo
            )
        {
            _productRepo = repo;
            ISessionFactory sessionFactory = connection.CreateSessionFactory();
            _session = sessionFactory.OpenSession();
            _categoryRepo = categoryRepo;
            _manufacturersRepo = manufacturersRepo;
            _productImagesRepository = productImageRepo;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IList<Products> productsList = _productRepo.GetAll();
            return View(productsList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            IList<Categories> categories = _categoryRepo.GetAll();
            IEnumerable<SelectListItem> categoryList =
                from c in categories
                select new SelectListItem
                {
                    Text = c.Name,
                    Value = c.ID.ToString()
                };

            IList<Manufacturers> manufacturers = _manufacturersRepo.GetAll();
            IEnumerable<SelectListItem> manufacturersList =
                from m in manufacturers
                select new SelectListItem
                {
                    Text = m.Name,
                    Value = m.ID.ToString()
                };

            ViewBag.Categories = categoryList;
            ViewBag.Manufacturers = manufacturersList;

            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductModel model)
        {
            bool isNotExist = _productRepo.FindDuplicateByName(model.Name);

            if (isNotExist)
            {
                Categories category = _categoryRepo.Get(Int32.Parse(model.Category));
                Manufacturers manufacturer = _manufacturersRepo.Get(Int32.Parse(model.Manufacturer));
                var product = new Products(model.Name, model.Description,
                    Convert.ToDecimal(model.Price, CultureInfo.InvariantCulture), category, manufacturer,
                    Int32.Parse(model.Quantity), model.IsFeatured, model.IsRecent, model.IsBestSeller, model.ShortDescription);

                try
                {
                    _productRepo.Save(product);
                    TempData["success"] = "Pomyślnie dodano nowy towar.";
                    return RedirectToAction("index");
                }
                catch (Exception)
                {
                    TempData["error"] = "Wystąpił problem z połączeniem do bazy danych.";
                    return RedirectToAction("Create");
                }
            }
            TempData["error"] = "Towar o podanej nazwie już istnieje";
            return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Products product = _productRepo.Get(id);
            if (product == null) throw new NullReferenceException();

            var productModel = new ProductModel
            {
                Id = product.ID.ToString(),
                Name = product.Name,
                Description = product.Description,
                Manufacturer = product.Manufacturer.Name,
                Category = product.Category.Name,
                Price = product.Price.ToString("F"),
                Quantity = product.Quantity.ToString()
            };

            ViewBag.HasImage = false;

            var image = _productImagesRepository.GetImage(id);

            if (image != null)
            {
                ViewBag.HasImage = true;
            }

            return View(productModel);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            Products product = _productRepo.Get(id);
            if (product == null) throw new NullReferenceException();

            var productModel = new ProductModel
            {
                Id = product.ID.ToString(),
                Name = product.Name,
                Description = product.Description,
                Manufacturer = product.Manufacturer.Name,
                Category = product.Category.Name,
                Price = product.Price.ToString("F"),
                Quantity = product.Quantity.ToString()
            };

            IList<Categories> categories = _categoryRepo.GetAll();
            IEnumerable<SelectListItem> categoryList = HtmlHelpers.CreateSelectList(categories, x => x.ID, x => x.Name);

            IList<Manufacturers> manufacturers = _manufacturersRepo.GetAll();
            IEnumerable<SelectListItem> manufacturersList = HtmlHelpers.CreateSelectList(manufacturers, x => x.ID,
                x => x.Name);

            ViewBag.Categories = categoryList;
            ViewBag.Manufacturers = manufacturersList;

            ViewBag.HasImage = false;

            var image = _productImagesRepository.GetImage(id);

            if (image != null)
            {
                ViewBag.HasImage = true;
            }

            return View(productModel);
        }

        [HttpPost]
        public ActionResult Update(ProductModel model)
        {
            bool isNotExist = _productRepo.FindDuplicateByNameAndID(model.Name, Int32.Parse(model.Id));
            bool isDecimal = false;
            decimal price = 0;
            int quantity = 0;
            if (decimal.TryParse(model.Price, out price) && Int32.TryParse(model.Quantity, out quantity))
            {
                isDecimal = true;
            }
            else
            {
                TempData["error"] = "Podana kwota nie jest wartością liczbową";
                return RedirectToAction("Update", new {@id = model.Id});
            }

            if (isNotExist && isDecimal)
            {
                Products product = _productRepo.Get(Int32.Parse(model.Id));

                if (product.Category.ID != Int32.Parse(model.Category))
                {
                    Categories category = _categoryRepo.Get(Int32.Parse(model.Category));
                    product.ChangeCategory(category);
                }
                if (product.Manufacturer.ID != Int32.Parse(model.Manufacturer))
                {
                    Manufacturers manufacturer = _manufacturersRepo.Get(Int32.Parse(model.Manufacturer));
                    product.ChangeManufacturer(manufacturer);
                }
                if (product.Name != model.Name ||
                    product.Description != model.Description ||
                    product.Price != Convert.ToDecimal(model.Price) ||
                    product.Quantity != Convert.ToInt32(model.Quantity))
                {
                    product.ChangeDetails(model.Name, model.Description, price, quantity, model.IsFeatured,
                        model.IsRecent, model.ShortDescription);
                }

                try
                {
                    _productRepo.Update(product);
                    TempData["success"] = String.Format("Edycja produktu {0} wykonana pomyślnie", product.Name);
                    return RedirectToAction("Details", new {@id = @product.ID});
                }
                catch (Exception)
                {
                    TempData["error"] = "Wystąpił problem z połączeniem do bazy danych.";
                    return RedirectToAction("Update", new {@id = @product.ID});
                }
            }
            TempData["error"] = String.Format("Produkt o nazwie {0} już istnieje", model.Name);
            return RedirectToAction("Update", new {@id = @model.Id});
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file, int productId)
        {
            if (file.ContentLength > 0)
            {
                if (file.ContentType.Contains("image"))
                {
                    var thePictureAsBytes = new byte[file.ContentLength];

                    using (var theReader = new BinaryReader(file.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(file.ContentLength);
                    }

                    Products product = _productRepo.Get(productId);
                    if (product == null) throw new Exception("Produkt o podanym id nie istnieje");

                    var productImage = _productImagesRepository.GetImage(productId);
                    var newProductImage = new ProductImages(file.FileName, thePictureAsBytes, product);
                    if (productImage == null)
                    {
                        _productImagesRepository.Save(newProductImage);
                    }
                    else
                    {
                        _productImagesRepository.Delete(productImage);
                        _productImagesRepository.Save(newProductImage);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wybrano błędne rozszerzenie pliku. Dopuszczalne pliki graficzne: jpg, jpeg, png");
                    return RedirectToAction("Update", "Products", new {id = productId});
                }
            }
            else
            {
                ViewBag.Message = "Musisz wybrać plik.";
            }
            return RedirectToAction("Update", new {@id = productId});
        }

        public FileContentResult ViewImage(int productID)
        {
            ProductImages image = _productImagesRepository.GetImage(productID);
            if (image == null) throw new Exception("Brak obrazka dla wskazanego produktu");
            byte[] buffer = image.ImageBytes;
            return new FileContentResult(buffer, "image/jpg");
        }
    }
}