using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using DataAccess.Entities;
using DataAccess.Repository;
using DataAccess.Repository.Interfaces;
using NHibernate;


namespace SklepInternetowy.Models
{
    public class ShoppingCartModel
    {
        public const string CartSessionKey = "CartId";
        private readonly IShoppingCartRepository _cartRepo;
        private readonly IOrdersRepository _orderRepo;
        private readonly IOrderDetailsRepository _orderDetailsRepo;
        private readonly IProductsRepository _producRepo;
        private string ShoppingCartId { get; set; }

        public ShoppingCartModel()
        {
            INhibernateConnection connection = new NHibernateConnection();
            ISessionFactory sessionFactory = connection.CreateSessionFactory();
            ISession _session = sessionFactory.OpenSession();
            _cartRepo = new ShoppingCartRepository(connection);
        }
        public static ShoppingCartModel GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCartModel();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public static ShoppingCartModel GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public string GetCartId(HttpContextBase context)
        {
            if(context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        public void AddToCart(Products product)
        {
            var cartItems = _cartRepo.GetAll();


            if (cartItems.Count == 0)
            {
                var cartItem = new ShoppingCarts(product, 1, ShoppingCartId, DateTime.Now);
                _cartRepo.Save(cartItem);
            }
            else if (cartItems.Count > 0)
            {
                var cartItem =
                    cartItems.FirstOrDefault(x => x.CartId == ShoppingCartId && x.Product.Name == product.Name);
                if (cartItem == null)
                {
                    cartItem = new ShoppingCarts(product, 1, ShoppingCartId, DateTime.Now);
                    _cartRepo.Save(cartItem);
                }
                else
                {
                    cartItem.Quantity++;
                }
                _cartRepo.Update(cartItem);
            }
        }


        public void RemoveFromCart(int id)
        {
            var cartItem = _cartRepo.GetAll().FirstOrDefault(x => x.CartId == ShoppingCartId);
           
            if (cartItem != null)
            {
                    _cartRepo.Delete(cartItem);
            }
        }

        public void EmptyCart()
        {
            var cartItems = _cartRepo.GetAll().Where(x => x.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _cartRepo.Delete(cartItem);
            }
        }


        public List<ShoppingCarts> GetCartItems()
        {
            return _cartRepo.GetAll().Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            var carts = _cartRepo.GetAll();
            
            int? count = (from cartItems in carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();
            
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            var carts = _cartRepo.GetAll();
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Quantity *
                              cartItems.Product.Price).Sum();

            return total ?? decimal.Zero;
        }

        public int CreateOrder(Orders order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var product = _producRepo.Get(item.ID);
                var ord = _orderRepo.Get(order.Id);
                var orderDetail = new OrderDetails(ord, product, item.Quantity,product.Price);
                
                // Set the order total of the shopping cart
                orderTotal += (item.Quantity * item.Product.Price);

                _orderDetailsRepo.Save(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.SummaryPrice = orderTotal;

            // Save the order
            _orderRepo.Save(order);
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.Id;
        }

        public void MigrateCart(string userName)
        {
            var shoppingCart = _cartRepo.GetAll().Where(x => x.CartId == ShoppingCartId);

            foreach (ShoppingCarts item in shoppingCart)
            {
                item.CartId = userName;
            }
        }

        public void UpdateItem(int id, int count)
        {
            var cartItems = _cartRepo.GetAll();
            var cartItem = cartItems.First(x => x.ID == id);

                if (cartItem != null)
                {
                    cartItem.Quantity = count;
                    _cartRepo.Update(cartItem);
                }
        }
    }
}