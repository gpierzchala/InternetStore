namespace DataAccess.Entities
{
    public class OrderDetails
    {
        protected OrderDetails() { }

        public OrderDetails(Orders order, Products product, int quantity, decimal unitPrice)
        {
            Order = order;
            Product = product;
            Quantity = quantity;
            UnitPrice = unitPrice;
            
        }

        public virtual int ID { get; set; }
        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal UnitPrice { get; set; }
        
    }
}
