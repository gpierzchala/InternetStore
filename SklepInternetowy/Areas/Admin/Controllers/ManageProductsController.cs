using System.Linq;
using System.Web.Mvc;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using SklepInternetowy.Areas.Admin.Models;
using System;
using System.IO;
using System.Web;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    public class ManageProductsController : Controller
    {
        private readonly IProductsRepository _productsRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IManufacturersRepository _manufacturerRepository;
        private readonly IProductImagesRepository _productImagesRepository;

        public ManageProductsController(IProductsRepository productsRepository, ICategoryRepository categoryRepository,
            IManufacturersRepository manufacturerRepository, IProductImagesRepository productImagesRepository)
        {
            _productsRepository = productsRepository;
            _categoryRepository = categoryRepository;
            _manufacturerRepository = manufacturerRepository;
            _productImagesRepository = productImagesRepository;
        }


        //ToDO: add update for products
        /// <summary>
        /// Method returns list of products connected with specific category id
        /// </summary>
        /// <param name="id">Category id</param>
        /// <returns></returns>
        public ActionResult ConnectedProductsWithCategory(int id)
        {
            var viewModel = new ConnectedProductsWithCategoryViewModel()
            {
                CategoryName = _categoryRepository.Get(id).Name,
                ProductData = _productsRepository.GetByCategory(id).Select(x => new ProductData
                {
                    Id = x.ID,
                    Name = x.Name,
                    IsBestseller = x.IsBestSeller,
                    IsFeatured = x.IsFeatured,
                    IsRecent = x.IsRecent
                }).ToList()
            };

            return View(viewModel);
        }

        public ActionResult ConnectedProductsWithManufacturers(int id)
        {
            var viewModel = new ConnectedProductsWithCategoryViewModel()
            {
                CategoryName = _manufacturerRepository.Get(id).Name,
                ProductData = _productsRepository.GetByManufacturer(id).Select(x => new ProductData
                {
                    Id = x.ID,
                    Name = x.Name,
                    IsBestseller = x.IsBestSeller,
                    IsFeatured = x.IsFeatured,
                    IsRecent = x.IsRecent
                }).ToList()
            };

            return View("ConnectedProductsWithCategory", viewModel);
        }


        public ActionResult Details(int id)
        {
            var product = _productsRepository.Get(id);

            if (product != null)
            {
                var viewModel = new ProductDetailsViewModel()
                {
                    ID = product.ID,
                    Name = product.Name,
                    AvailableItemCount = product.Quantity,
                    Category = product.Category.Name,
                    Description = product.Description,
                    Manufacturer = product.Manufacturer.Name,
                    Price = product.Price,
                    IsBestseller = product.IsBestSeller,
                    IsFeatured = product.IsFeatured,
                    IsRecent = product.IsRecent
                };
                return View(viewModel);
            }

            return new HttpNotFoundResult("Nie udało onaleźć się wybranego produktu");
        }

        public ActionResult List()
        {
            var viewModel = (_productsRepository.GetAll().Select(x => new ProductListViewModel
            {
                Category = x.Category.Name,
                Name = x.Name,
                Price = x.Price,
                Id = x.ID,
                Manufacturer = x.Manufacturer.Name
            })).ToList();

            ViewBag.Manufacturer =
                _manufacturerRepository.GetAll()
                    .Select(x => new SelectListItem() {Text = x.Name, Value = x.ID.ToString()})
                    .ToList();
            ViewBag.Category =
                _categoryRepository.GetAll()
                    .Select(x => new SelectListItem() {Text = x.Name, Value = x.ID.ToString()})
                    .ToList();

            return View(viewModel);
        }

        public ActionResult Update(int id)
        {
            var product = _productsRepository.Get(id);
            var viewModel = new CreateProductModel()
            {
                Category = product.Category.Name,
                IsBestseller = product.IsBestSeller,
                Name = product.Name,
                Price = product.Price.ToString(),
                IsFeatured = product.IsFeatured,
                IsRecent = product.IsRecent,
                Manufacturer = product.Manufacturer.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                ShortDescription = product.ShortDescription
            };

            var manufacturers =
                _manufacturerRepository.GetAll()
                    .Select(x => new SelectListItem() {Text = x.Name, Value = x.ID.ToString()})
                    .ToList();

            foreach (var manufacturer in manufacturers)
            {
                if (manufacturer.Text == viewModel.Manufacturer)
                    manufacturer.Selected = true;
            }

            ViewBag.Manufacturer = manufacturers;

            var categories =
                _categoryRepository.GetAll()
                    .Select(
                        x =>
                            new SelectListItem()
                            {
                                Text = x.Name,
                                Value = x.ID.ToString(),
                                Selected = x.Name == viewModel.Manufacturer
                            })
                    .ToList();

            foreach (var category in categories)
            {
                if (category.Text == viewModel.Category)
                    category.Selected = true;
            }
            ViewBag.ProductId = id;
            ViewBag.Category = categories;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(CreateProductModel model, int id)
        {
            bool isNotExist = _productsRepository.FindDuplicateByNameAndID(model.Name, id);

            if (isNotExist)
            {
                var categoryId = Convert.ToInt32(model.Category);
                var manufacturerId = Convert.ToInt32(model.Manufacturer);
                var price = model.Price.Contains(".")
                    ? Convert.ToDecimal(model.Price.Replace('.', ','))
                    : Convert.ToDecimal(model.Price);

                var newProduct = new Products(model.Name, model.Description, price, _categoryRepository.Get(categoryId),
                    _manufacturerRepository.Get(manufacturerId), model.Quantity, model.IsFeatured, model.IsRecent,
                    model.IsBestseller, model.ShortDescription) {ID = id};
                try
                {
                    _productsRepository.Update(newProduct);
                    TempData["success"] = String.Format("Produkt {0} został edytowany pomyślnie", newProduct.Name);
                }
                catch (Exception)
                {
                    throw new Exception();
                }
            }
            else
            {
                TempData["error"] = String.Format("Produkt o nazwie {0} już istnieje", model.Name);
            }


            return RedirectToAction("Update", new {id = id});
        }

        [HttpPost]
        public ActionResult Create(CreateProductModel model)
        {
            bool isNotExist = _productsRepository.FindDuplicateByName(model.Name);

            if (isNotExist)
            {
                var categoryId = Convert.ToInt32(model.Category);
                var manufacturerId = Convert.ToInt32(model.Manufacturer);
                var price = model.Price.Contains(".")
                    ? Convert.ToDecimal(model.Price.Replace('.', ','))
                    : Convert.ToDecimal(model.Price);

                var newProduct = new Products(model.Name, model.Description ?? String.Empty, price,
                    _categoryRepository.Get(categoryId), _manufacturerRepository.Get(manufacturerId), model.Quantity,
                    model.IsFeatured, model.IsRecent, model.IsBestseller, model.ShortDescription);

                try
                {
                    _productsRepository.Save(newProduct);
                    if (model.Photo != null)
                    {
                        var target = new MemoryStream();
                        model.Photo.InputStream.CopyTo(target);
                        var data = target.ToArray();
                        var productImage = new ProductImages(model.Photo.FileName, data, newProduct);
                        _productImagesRepository.Save(productImage);
                    }


                    TempData["success"] = String.Format("Produkt {0} została utworzona pomyślnie",
                        newProduct.Name);
                }
                catch (Exception)
                {
                    throw new Exception();
                }


                return RedirectToAction("List");
            }
            TempData["error"] = String.Format("Produkt o nazwie {0} już istnieje", model.Name);
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult ChangeImage(int productId, HttpPostedFileBase photo)
        {
            var product = _productsRepository.Get(productId);

            if (product != null && photo != null)
            {
                var target = new MemoryStream();
                photo.InputStream.CopyTo(target);
                var data = target.ToArray();
                var productImage = new ProductImages(photo.FileName, data, product);
                _productImagesRepository.Save(productImage);
            }


            return RedirectToAction("Details", new {id = productId});
        }

        public FileContentResult ViewImage(int productID)
        {
            ProductImages image = _productImagesRepository.GetImage(productID);
            if (image == null)
            {
                string path = Server.MapPath(Url.Content("~/FrontEnd/img/empty_gallery.png"));
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