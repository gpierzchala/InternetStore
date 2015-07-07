using System;
using DataAccess.Entities;

namespace SklepInternetowy.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public string CartId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Products Product { get; set; }
    }
}