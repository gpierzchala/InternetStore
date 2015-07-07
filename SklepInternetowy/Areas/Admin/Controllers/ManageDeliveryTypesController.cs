using System;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using SklepInternetowy.Areas.Admin.Models;


namespace SklepInternetowy.Areas.Admin.Controllers
{
    public class ManageDeliveryTypesController : Controller
    {
        private readonly IDeliveryTypesRepository _deliveryTypesRepository;

        public ManageDeliveryTypesController(IDeliveryTypesRepository deliveryTypesRepository)
        {
            _deliveryTypesRepository = deliveryTypesRepository;
        }

        public ActionResult List()
        {
            var viewModel = _deliveryTypesRepository.GetAll().Select(x => new DeliveryTypeViewModel
            {
                Id = x.ID,
                Name = x.Name,
                Price = x.Price.ToString("C")
            }).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(DeliveryTypeViewModel model)
        {
            bool isNotExist = _deliveryTypesRepository.FindDuplicateByName(model.Name);

            if (isNotExist)
            {
                decimal price = model.Price.Contains(".")
                    ? Convert.ToDecimal(model.Price.Replace('.', ','))
                    : Convert.ToDecimal(model.Price);
                var newDeliveryTYpe = new DeliveryTypes(model.Name, price);

                try
                {
                    _deliveryTypesRepository.Save(newDeliveryTYpe);
                    TempData["success"] = String.Format("Kategoria {0} została utworzona pomyślnie",
                        newDeliveryTYpe.Name);
                }
                catch (Exception)
                {
                    throw new Exception();
                }
                return RedirectToAction("List");
            }
            TempData["error"] = String.Format("Producent o nazwie {0} już istnieje", model.Name);
            return RedirectToAction("Create");
        }

        public ActionResult Update(int id)
        {
            var delivery = _deliveryTypesRepository.Get(id);
            if (delivery == null) return RedirectToAction("List", "ManageManufacturers");

            var model = new DeliveryTypeViewModel
            {
                Name = delivery.Name,
                Id = delivery.ID,
                Price = Math.Round(delivery.Price, 2).ToString()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Update(DeliveryTypeViewModel model)
        {
           
                bool isNotExist = _deliveryTypesRepository.FindDuplicateByNameAndId(model.Name,model.Id);

                if (isNotExist)
                {
                    decimal price = model.Price.Contains(".")
                    ? Convert.ToDecimal(model.Price.Replace('.', ','))
                    : Convert.ToDecimal(model.Price);
                    var deliveryType = _deliveryTypesRepository.Get(model.Id);
                    deliveryType.Update(model.Name, price);

                    try
                    {
                        _deliveryTypesRepository.Update(deliveryType);
                        TempData["success"] = String.Format("Edycja producenta {0} wykonana pomyślnie",
                            deliveryType.Name);
                        return RedirectToAction("List");
                    }
                    catch (Exception)
                    {
                        TempData["error"] = "Wystąpił problem z połączeniem do bazy danych.";
                        return RedirectToAction("Update", new { @id = model.Id });
                    }
                }
                TempData["error"] = String.Format("Producent o nazwie {0} już istnieje", model.Name);

            return RedirectToAction("Update", new { @id = @model.Id });
        }
    }
}