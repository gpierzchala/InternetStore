using System;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using SklepInternetowy.Models;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    public class ManageManufacturersController : Controller
    {
        private readonly IManufacturersRepository _manufacturersRepository;
        private readonly IProductsRepository _productsRepository;

        public ManageManufacturersController(IManufacturersRepository manufacturersRepository,
            IProductsRepository productsRepository)
        {
            _manufacturersRepository = manufacturersRepository;
            _productsRepository = productsRepository;
        }

        public ActionResult List()
        {
            var manufacturers = _manufacturersRepository.GetAll().Select(x => new ManufacturerViewModel
            {
                Id = x.ID,
                Name = x.Name
            }).ToList();


            return View(manufacturers);
        }

        public ActionResult Update(int id)
        {
            var manufacturer = _manufacturersRepository.Get(id);
            if (manufacturer == null) return RedirectToAction("List", "ManageManufacturers");

            var categoryModel = new ManufacturerViewModel
            {
                Name = manufacturer.Name,
                Id = manufacturer.ID
            };

            return View(categoryModel);
        }

        [HttpPost]
        public ActionResult Update(ManufacturerViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNotExist = _manufacturersRepository.FindDuplicateByNameAndId(model.Name, model.Id);

                if (isNotExist)
                {
                    var manufacturer = _manufacturersRepository.Get(model.Id);
                    manufacturer.Update(model.Name);

                    try
                    {
                        _manufacturersRepository.Update(manufacturer);
                        TempData["success"] = String.Format("Edycja producenta {0} wykonana pomyślnie",
                            manufacturer.Name);
                        return RedirectToAction("List");
                    }
                    catch (Exception)
                    {
                        TempData["error"] = "Wystąpił problem z połączeniem do bazy danych.";
                        return RedirectToAction("Update", new {@id = model.Id});
                    }
                }
                TempData["error"] = String.Format("Producent o nazwie {0} już istnieje", model.Name);
            }
            else
            {
                TempData["error"] = "Nazwa producenta jest wymagana";
            }

            return RedirectToAction("Update", new {@id = @model.Id});
        }


        [HttpPost]
        public ActionResult Delete(int manufacturerId)
        {
            var manufacturer = _manufacturersRepository.Get(manufacturerId);

            var isNotOtherExist = _manufacturersRepository.FindDuplicateByName("Pozostali");

            if (isNotOtherExist)
            {
                _manufacturersRepository.Save(new Manufacturers("Pozostali"));
            }

            var newManufacturer = _manufacturersRepository.GetAll().FirstOrDefault(x => x.Name.Contains("Pozostali"));

            if (manufacturer != null && newManufacturer != null)
            {
                var products = _productsRepository.GetByManufacturer(manufacturer.ID);

                foreach (var product in products)
                {
                    product.Manufacturer = newManufacturer;
                    _productsRepository.Update(product);
                }

                _manufacturersRepository.Delete(manufacturer);
                TempData["success"] = String.Format("Pomyślnie usunięto kategorie {0}", manufacturer.Name);
            }
            else
            {
                throw new Exception("Brak producenta z numerem id" + manufacturerId);
            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Create(string manuName)
        {

            bool isNotExist = _manufacturersRepository.FindDuplicateByName(manuName);

                if (isNotExist)
                {
                    var newManufacturer = new Manufacturers(manuName);

                    try
                    {
                        _manufacturersRepository.Save(newManufacturer);
                        TempData["success"] = String.Format("Kategoria {0} została utworzona pomyślnie",
                            newManufacturer.Name);
                    }
                    catch (Exception)
                    {
                        throw new Exception();
                    }
                    return RedirectToAction("List");
                }
                TempData["error"] = String.Format("Producent o nazwie {0} już istnieje", manuName);
                return RedirectToAction("Create");
            
        }
    }
}