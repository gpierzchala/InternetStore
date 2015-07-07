using System;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Repository.Interfaces;
using SklepInternetowy.Areas.Admin.Models;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    public class ManageUsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public ManageUsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ActionResult List()
        {
            var users = (_userRepository.GetAllUsers().Select(x => new UserModel
            {
                Name = x.Name + x.Surname,
                Email = x.Email,
                Address = x.Address + x.ZipCode + " " + x.City,
                IsAdmin = x.IsAdmin
            })).ToList();

            return View(users);
        }

        public ActionResult ChangeUserRole(string email)
        {
            _userRepository.ChangeUserRole(email);
            TempData["success"] = String.Format("Edycja roli użytkownika z adresem email {0} wykonana pomyślnie",
                    email);
            return RedirectToAction("List");
        }

    }
}