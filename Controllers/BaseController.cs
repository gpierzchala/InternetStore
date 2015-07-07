using System.Collections.Generic;
using System.Web.Mvc;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using SklepInternetowy.Helpers;

namespace SklepInternetowy.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ICategoryRepository _catRepo;
        protected readonly IManufacturersRepository _manufacturersRepository;
        protected readonly IOrdersRepository _ordersRepository;
        protected readonly IUserRepository _userRepository;
        protected readonly IOrderDetailsRepository _orderDetailsRespo;
        protected readonly IProductsRepository _productsRepository;
        protected BaseController(
            ICategoryRepository catRepo,
            IManufacturersRepository manufacturersRepository)
        {
            _catRepo = catRepo;
            _manufacturersRepository = manufacturersRepository;
        }

        protected BaseController(
            ICategoryRepository categoryRepository,
            IManufacturersRepository manufacturersRepository,
            IProductsRepository productsRepository)
        {
            _catRepo = categoryRepository;
            _manufacturersRepository = manufacturersRepository;
            _productsRepository = productsRepository;
        }

        protected BaseController(
            IOrdersRepository ordersRepository, 
            IOrderDetailsRepository orderDetailsRepository,
            IUserRepository usersRepository,
            ICategoryRepository catRepo,
            IManufacturersRepository manufacturersRepository)
        {
            _userRepository = usersRepository;
            _ordersRepository = ordersRepository;
            _orderDetailsRespo = orderDetailsRepository;
            _catRepo = catRepo;
            _manufacturersRepository = manufacturersRepository;
        }

        public IList<Categories> GetCategories()
        {
            return _catRepo.GetAll();
        }

        public IEnumerable<SelectListItem> GetCategoryList()
        {
            IList<Categories> categories = _catRepo.GetAll();
            return HtmlHelpers.CreateSelectList(categories, x => x.ID, x => x.Name);
        }

        public IList<Manufacturers> GetManufacturers()
        {
            return _manufacturersRepository.GetAll();
        }
    }
}