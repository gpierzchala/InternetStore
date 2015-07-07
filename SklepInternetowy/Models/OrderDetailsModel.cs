using System.Collections.Generic;
using DataAccess.Entities;

namespace SklepInternetowy.Models
{
    public class OrderDetailsModel
    {
        public Orders Order { get; set; }
        public List<OrderDetails> OrderDetails { get; set; } 
    }
}