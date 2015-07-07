using System.Collections.Generic;
using DataAccess.Entities;

namespace SklepInternetowy.Models
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCarts> CartItems { get; set; }
        public decimal CartTotal { get; set; }
        public string DeliveryType { get; set; }
        public decimal DeliveryCost { get; set; }
        public int ItemCount { get; set; }
    }
}