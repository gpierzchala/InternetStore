using System;

namespace DataAccess.Entities
{
    public class ShoppingCarts
    {
        protected ShoppingCarts()
        {}

        public ShoppingCarts(Products product, int quantity, string cartId)
        {
            Product = product;
            Quantity = quantity;
            DateCreated = DateTime.Now;
            CartId = cartId;
        }

        public virtual int ID { get; set; }
        public virtual string CartId { get; set; }
        public virtual Products Product { get; set; }
        public virtual int Quantity { get; set; }
        public virtual DateTime DateCreated { get; set; }
    }
}
