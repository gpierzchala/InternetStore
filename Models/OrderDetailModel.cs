using DataAccess.Entities;

namespace SklepInternetowy.Models
{
    public class OrderDetailModel
    {
        public int ID { get; set; }
        public int OrderId { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Products Album { get; set; }
        public virtual OrderModel Order { get; set; }
    }
}