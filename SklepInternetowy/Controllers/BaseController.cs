using System.Collections.Generic;
using System.Web.Mvc;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using SklepInternetowy.Helpers;

namespace SklepInternetowy.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ICategoryRepository CatRepo;
        protected readonly IManufacturersRepository ManufacturersRepository;
        protected readonly IOrdersRepository OrdersRepository;
        protected readonly IUserRepository UserRepository;
        protected readonly IOrderDetailsRepository OrderDetailsRespo;
        protected readonly IProductsRepository ProductsRepository;
        protected BaseController(
            ICategoryRepository catRepo,
            IManufacturersRepository manufacturersRepository)
        {
            CatRepo = catRepo;
            ManufacturersRepository = manufacturersRepository;
            AutoLoginRememberedUser();
        }

        protected BaseController(
            ICategoryRepository categoryRepository,
            IManufacturersRepository manufacturersRepository,
            IProductsRepository productsRepository)
        {
            CatRepo = categoryRepository;
            ManufacturersRepository = manufacturersRepository;
            ProductsRepository = productsRepository;
            AutoLoginRememberedUser();
        }

        protected BaseController(
            IOrdersRepository ordersRepository, 
            IOrderDetailsRepository orderDetailsRepository,
            IUserRepository usersRepository,
            ICategoryRepository catRepo,
            IManufacturersRepository manufacturersRepository)
        {
            UserRepository = usersRepository;
            OrdersRepository = ordersRepository;
            OrderDetailsRespo = orderDetailsRepository;
            CatRepo = catRepo;
            ManufacturersRepository = manufacturersRepository;
            AutoLoginRememberedUser();
        }

        public IList<Categories> GetCategories()
        {
            return CatRepo.GetAll();
        }

        private void AutoLoginRememberedUser()
        {
          //  var isRemembered = Response.Cookies["rememberMe"] != null ? Request.Cookies["rememberMe"].Value : null;

            // FormsAuthentication.SetAuthCookie(user.Email, model.RememberMe);
        }

        public IEnumerable<SelectListItem> GetCategoryList()
        {
            IList<Categories> categories = CatRepo.GetAll();
            return HtmlHelpers.CreateSelectList(categories, x => x.ID, x => x.Name);
        }

        public IList<Manufacturers> GetManufacturers()
        {
            return ManufacturersRepository.GetAll();
        }
    }
}