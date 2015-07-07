using System;
using System.Collections.Generic;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal SummaryPrice { get; set; }
        public string Username { get; set; }
        public string DeliveryType { get; set; }
        public decimal Total { get; set; }
        public string OrderState { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string DeliveryAddress { get; set; }
        public IList<OrderDetailsModel> OrderDetails { get; set; }
    }
}