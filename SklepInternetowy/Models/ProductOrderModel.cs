namespace SklepInternetowy.Models
{
    public class ProductOrderModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal PriceForPiece { get; set; }
    }
}