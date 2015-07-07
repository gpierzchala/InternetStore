using System.Web.Mvc;
using System.Web.Security;
using DataAccess.Repository.Interfaces;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserRepository _userRepository;

        private bool isAdmin = false;
        public HomeController(IUserRepository userRepository)
        {
            
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home", new {area = ""});
        }
	}
}