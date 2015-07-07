using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;
using PagedList;
using SklepInternetowy.Models;

namespace SklepInternetowy.Helpers
{
    public class Common
    {
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductsRepository _productRepo;
        private readonly IDeliveryTypesRepository _deliveryTypesRepo;
        private readonly IManufacturersRepository _manufacturersRepository;
        public Common(ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }

        public Common(IDeliveryTypesRepository deliveryRepo)
        {
            _deliveryTypesRepo = deliveryRepo;
        }

        public Common(IManufacturersRepository manufacturersRepository)
        {
            _manufacturersRepository = manufacturersRepository;
        }

        /// <summary>
        ///     Get all categories available in DB
        /// </summary>
        /// <returns>Categories list</returns>
        public IList<Categories> GetCategories()
        {
            return _categoryRepo.GetAll();
        }

        public IList<Manufacturers> GetManufacturers()
        {
            return _manufacturersRepository.GetAll();
        }


        /// <summary>
        ///     Get all categories and parse it to select list
        /// </summary>
        /// <returns>Categories selectList items</returns>
        public IEnumerable<SelectListItem> GetCategoryList()
        {
            IList<Categories> categories = _categoryRepo.GetAll();
            return HtmlHelpers.CreateSelectList(categories, x => x.ID, x => x.Name);
        }


        /// <summary>
        ///     Sort collection by special parameter
        /// </summary>
        /// <typeparam name="T">Products</typeparam>
        /// <param name="collection">Products collection</param>
        /// <param name="searchOption">Specific sort option</param>
        /// <returns>Sorted passed collection</returns>
        public static IEnumerable<T> SortCollection<T>(IEnumerable<T> collection, int? searchOption) where T : Products
        {
            if (searchOption == 1)
            {
                // po nazwie A-Z
                collection = collection.OrderBy(x => x.Name);
            }
            else if (searchOption == 2)
            {
                // po nazwie Z-A
                collection = collection.OrderByDescending(x => x.Name);
            }
            else if (searchOption == 3)
            {
                // po cenie rosnacej
                collection = collection.OrderBy(x => x.Price);
            }
            else if (searchOption == 4)
            {
                // po cenie malejacej
                collection = collection.OrderByDescending(x => x.Price);
            }
            else if (searchOption == 5)
            {
                // po dostepnej ilosci
                collection = collection.OrderByDescending(x => x.Quantity);
            }

            return collection;
        }


        /// <summary>
        ///     Sort and pagin specific collection
        /// </summary>
        /// <typeparam name="T">Products</typeparam>
        /// <param name="sortOption">Type of sorting </param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size if pageSize == null then it take default value equals 12</param>
        /// <param name="items">Specific collection of products</param>
        /// <returns>One page of collecion</returns>
        public static IPagedList<T>     SortAndPagin<T>(int? sortOption, int? page, int? pageSize, IEnumerable<T> items)
            where T : Products
        {
            IEnumerable<T> collection = items;

            if (sortOption != null)
            {
                collection = SortCollection(collection, sortOption);
            }

            int specialProductsPage = page ?? 1;
            IPagedList<T> onePageOfProducts = collection.ToPagedList(specialProductsPage, pageSize ?? 12);
            return onePageOfProducts;
        }

        /// <summary>
        ///     Generate available sort options
        /// </summary>
        /// <returns>List of available sort options</returns>
        public static SearcherModel[] GetSortOptions()
        {
            return new[]
            {
                new SearcherModel {ID = 1, Name = "Nazwy A-Z"},
                new SearcherModel {ID = 2, Name = "Nazwy Z-A"},
                new SearcherModel {ID = 3, Name = "Najniższa cena"},
                new SearcherModel {ID = 4, Name = "Najwyższa cena"},
                new SearcherModel {ID = 5, Name = "Dostępnej ilości"}
            };
        }

        /// <summary>
        /// Generate random bool value
        /// </summary>
        /// <returns>Random bool value</returns>
        public static bool GetRandomBool()
        {
            System.Threading.Thread.Sleep(100);
            Random rnd = new Random();
            return rnd.Next(0, 2) == 0;
        }

        public IEnumerable<SelectListItem> GetDeliverTypes()
        {
            IList<DeliveryTypes> deliveryTypes = _deliveryTypesRepo.GetAll();
            return HtmlHelpers.CreateSelectList(deliveryTypes, x => x.ID, x => x.Name + " " + x.Price.ToString("F") + " zł.");
        }
    }
}