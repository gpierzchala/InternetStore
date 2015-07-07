using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccess.Repository.Interfaces;
using SklepInternetowy.Areas.Admin.Models;

namespace SklepInternetowy.Areas.Admin.Controllers
{
    public class ManageOrdersController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IOrderDetailsRepository _orderDetailsRepository;
        private readonly IOrderStateRepository _orderStateRepository;

        public ManageOrdersController(IOrdersRepository ordersRepository, IOrderStateRepository orderStateRepository,
            IOrderDetailsRepository orderDetailsRepository)
        {
            _ordersRepository = ordersRepository;
            _orderStateRepository = orderStateRepository;
            _orderDetailsRepository = orderDetailsRepository;
        }

        public ActionResult List()
        {
            var orders = (_ordersRepository.GetAll().Select(x => new OrderModel()
            {
                OrderId = x.Id,
                Username = x.User.Email,
                OrderDate = x.OrderDate,
                Total = x.SummaryPrice,
                OrderState = x.State.StateName,
            })).ToList();

            ViewBag.OrderStates =
                _orderStateRepository.GetAll()
                    .Select(x => new SelectListItem() {Text = x.StateName, Value = x.ID.ToString()})
                    .ToList();

            return View(orders);
        }

        public ActionResult Details(int id)
        {
            var order = _ordersRepository.Get(id);
            var details = _orderDetailsRepository.GetForOrder(id);
            OrderModel model = new OrderModel();
            if (order != null && details != null)
            {
                model.OrderId = order.Id;
                model.FirstName = order.User.Name;
                model.LastName = order.User.Surname;
                model.OrderDate = order.OrderDate;
                model.Email = order.User.Email;
                model.DeliveryAddress = order.User.Address + order.User.ZipCode + " " + order.User.City;
                model.SummaryPrice = order.SummaryPrice;
                model.OrderDetails = new List<OrderDetailsModel>();
                model.OrderState = order.State.StateName;
                foreach (var item in details)
                {
                    model.OrderDetails.Add(new OrderDetailsModel()
                    {
                        ProductName = item.Product.Name,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    });
                }
            }
            return View(model);
        }

        public ActionResult UserOrders(string email)
        {
            var orders = _ordersRepository.GetUserOrders(email);

            var model = new List<OrderModel>();
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    var o = new OrderModel()
                    {
                        OrderId = order.Id,
                        FirstName = order.User.Name,
                        LastName = order.User.Surname,
                        OrderDate = order.OrderDate,
                        Email = order.User.Email,
                        DeliveryAddress = order.User.Address + order.User.ZipCode + " " + order.User.City,
                        SummaryPrice = order.SummaryPrice,
                        OrderDetails = new List<OrderDetailsModel>(),
                        OrderState = order.State.StateName,
                        DeliveryType = order.DeliveryType.Name
                    };


                    var details = _orderDetailsRepository.GetForOrder(order.Id);

                    if (details != null)
                    {
                        foreach (var item in details)
                        {
                            o.OrderDetails.Add(new OrderDetailsModel()
                            {
                                ProductName = item.Product.Name,
                                Quantity = item.Quantity,
                                UnitPrice = item.UnitPrice
                            });
                        }
                    }

                    model.Add(o);
                }
            }

            return View(model);
        }

        public ActionResult ConnectedOrdersWithProducts(int id)
        {
            var details = _orderDetailsRepository.GetAll().Where(x => x.Product.ID == id).Select(x => x.Order.Id);
            var containsDetails = _ordersRepository.GetAll().Where(x => details.Contains(x.Id)).ToList();
            var orders = (containsDetails.Select(x => new OrderModel()
            {
                OrderId = x.Id,
                Username = x.User.Email,
                OrderDate = x.OrderDate,
                Total = x.SummaryPrice,
                OrderState = x.State.StateName,
            })).ToList();

            ViewBag.OrderStates =
                _orderStateRepository.GetAll()
                    .Select(x => new SelectListItem() {Text = x.StateName, Value = x.ID.ToString()})
                    .ToList();

            return View("List", orders);
        }

        public ActionResult ChangeOrderState(int orderId, int orderStates)
        {
            if (_ordersRepository.UpdateState(orderId, orderStates))
            {
                TempData["success"] = String.Format("Edycja stanu zamówienia o numerze {0} wykonana pomyślnie",
                    orderId);
            }

            return RedirectToAction("Details", new {id = orderId});
        }
    }
}