using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using NHibernate;
using SklepInternetowy.Areas.Admin.Models;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    [Authorize]
    public class ManageCategoriesController : Controller
    {
        private readonly ISession _session;
        private IList<Categories> categoryList = null;
        private readonly ICategoryRepository _categoryRepository;
        public ManageCategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public ActionResult List()
        {
            IList<Categories> categoryList = _categoryRepository.GetAll()
                                                .OrderBy(x => x.Name).ToList();
            return View(categoryList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNotExist = _categoryRepository.FindDuplicateByName(model.Name);

                if (isNotExist)
                {
                    var newCategory = new Categories(model.Name, model.Description);

                    try
                    {
                        _categoryRepository.Save(newCategory);
                        TempData["success"] = String.Format("Kategoria {0} została utworzona pomyślnie",
                            newCategory.Name);
                    }
                    catch (Exception)
                    {
                        throw new Exception();
                    }
                    return RedirectToAction("List");
                }
                TempData["error"] = String.Format("Kategoria o nazwie {0} już istnieje", model.Name);
                return RedirectToAction("Create");
            }
            TempData["error"] = "Zostały błednie wprowadzone dane";
            return RedirectToAction("Create");
        }

        //[HttpGet]
        //public ActionResult Delete(int id)
        //{
        //    var category = _categoryRepository.Get(id);

        //    if (category == null)
        //        return HttpNotFound();

        //    DeleteConfirmationViewModel model = new DeleteConfirmationViewModel
        //    {
        //        DeleteEntityId = id,
        //        HeaderText = category.Name,
        //        PostDeleteAction = "Delete",
        //        PostDeleteController = "ManageCategories"
        //    };
        //    return PartialView("_DeleteConfirmation", model);
        //}

        [HttpPost]
        public ActionResult Delete(int catId)
        {
            var category = _categoryRepository.Get(catId);

            if (category != null)
            {
                _categoryRepository.Delete(category);
                TempData["success"] = String.Format("Pomyślnie usunięto kategorie {0}", category.Name);
            }
            else
            {
                throw new Exception("Brak kategorii z numerem id" + catId);
            }

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            Categories category = _categoryRepository.GetAll().FirstOrDefault(x => x.ID == id);
            if (category == null) return RedirectToAction("List", "ManageCategories");

            var categoryModel = new CategoryModel
            {
                Name = category.Name,
                Description = category.Description
            };

            return View(categoryModel);
        }

        [HttpPost]
        public ActionResult Update(CategoryModel model)
        {
            bool isNotExist = _categoryRepository.FindDuplicateByNameAndId(model.Name, Int32.Parse(model.Id));

            if (isNotExist)
            {
                Categories category = _categoryRepository.Get(Int32.Parse(model.Id));
                category.Update(model.Name, model.Description);

                try
                {
                    _categoryRepository.Update(category);
                    TempData["success"] = String.Format("Edycja kategorii {0} wykonana pomyślnie", category.Name);
                    return RedirectToAction("List");
                }
                catch (Exception)
                {
                    TempData["error"] = "Wystąpił problem z połączeniem do bazy danych.";
                    return RedirectToAction("Update", new {@id = model.Id});
                }
            }
            TempData["error"] = String.Format("Kategoria o nazwie {0} już istnieje", model.Name);
            return RedirectToAction("Update", new {@id = @model.Id});
        }
    }
}