using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BusinessLogic.MyAccount;
using DataAccess.Repository.Interfaces;
using SimpleCrypto;
using SklepInternetowy.Helpers;
using SklepInternetowy.Models;
using RegisterModel = BusinessLogic.Models.RegisterModel;

namespace SklepInternetowy.Controllers
{
    [Authorize]
    public class MyAccountController : BaseController
    {
        private readonly HttpCookie _cookie;
        private readonly HttpCookie _isAdminCookie;
        private readonly MyAccount myAccountLogic;
        
        public MyAccountController(
            IUserRepository userRepo,
            IOrdersRepository orderRepo,
            IOrderDetailsRepository orderDetailsRespo,
            ICategoryRepository catRepo,
            IManufacturersRepository manufacturersRepository) 
            : base(orderRepo,orderDetailsRespo,userRepo,catRepo,manufacturersRepository)
        {
            myAccountLogic = new MyAccount(userRepo);
            _cookie = new HttpCookie("userId");
            _isAdminCookie = new HttpCookie("isAdmin");
            ViewBag.Categories = GetCategories();
            ViewBag.Manufacturers = GetManufacturers();
            ViewBag.CategorySelectList = GetCategoryList();
            ViewBag.IsAdmin = false;
        }
        
        public ActionResult MyAccount()
        {
            var user = myAccountLogic.GetUser(HttpContext.User.Identity.Name);
            return View(user);
        }

        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = myAccountLogic.GetUser(model.Email);

                if (user != null)
                {
                    bool isValid = CryptoHelpers.IsValid(user, model.Password);
                    if (isValid && user.IsAdmin)
                    {
                        FormsAuthentication.SetAuthCookie(user.Email, model.RememberMe);
                        _cookie.Value = user.ID.ToString();
                        _cookie.Expires = DateTime.Now.AddHours(3);
                        Response.Cookies.Add(_cookie);
                        _isAdminCookie.Value = "1";
                        _isAdminCookie.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Add(_isAdminCookie);
                        return RedirectToAction("Index", "Home", new {area = "Admin"});
                    }
                    if (isValid)
                    {
                        FormsAuthentication.SetAuthCookie(user.Email, model.RememberMe);
                        _cookie.Value = user.ID.ToString();
                        Response.Cookies.Add(_cookie);
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("", "Wprowadzone dane są niepoprawne.");
                    return View("Login");
                }
            }
            else
            {
                ModelState.AddModelError("", "Wprowadzone dane są niepoprawne.");
                return View("Login");
            }
            return RedirectToAction("Index", "Home");
        }

        
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();

            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            var userCartItems = ShoppingCartModel.GetCart(HttpContext);
            userCartItems.EmptyCart();

            return RedirectToAction("Index", "Home");
        }

        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                bool isNotExist = myAccountLogic.FindDuplicateUser(model.Email);

                if (isNotExist)
                {
                    var crypto = new PBKDF2();
                    string enryptPass = crypto.Compute(model.Password);
                    string passwordSalt = crypto.Salt;
                    string address = model.Street + " " + model.HouseNumber;
                    string ipaddress = System.Web.HttpContext.Current.Request.UserHostAddress;

                    if (model.FlatNumber != null)
                    {
                        address += "/" + model.FlatNumber;
                    }

                    if (myAccountLogic.IsSuccessedCreatedUser(model, enryptPass, address, false, passwordSalt, ipaddress))
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                       ModelState.AddModelError("", "Wystąpił problem z serwerem, proszę spróbować ponownie.");
                        return View("Register"); 
                    }
                }
                ModelState.AddModelError("", String.Format("Użytkownik o adresie email {0} już istnieje", model.Email));
                return View("Register");
            }
            ModelState.AddModelError("", "Wprowadzono błędne wartości");
            return View("Register");
        }

        
        public ActionResult AccountDetails()
        {
            var user = myAccountLogic.GetUser(HttpContext.User.Identity.Name);

            var userModel = new UserModel
            {
                Name = user.Name,
                Email = user.Email,
                City = user.City,
                Address = user.Address,
                Surname = user.Surname,
                ZipCode = user.ZipCode
            };

            return View(userModel);
        }

       
        public ActionResult Orders()
        {
            var user = _userRepository.Get(HttpContext.User.Identity.Name);

            var userOrders = _ordersRepository.GetAll(user.ID);
            return View(userOrders);
        }

        public ActionResult OrderDetails(int id)
        {
            var order = _ordersRepository.Get(id);
            var orderDetails = _orderDetailsRespo.Get(order.Id);

            if (order.User.Email == HttpContext.User.Identity.Name)
            {
                return View(orderDetails);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}