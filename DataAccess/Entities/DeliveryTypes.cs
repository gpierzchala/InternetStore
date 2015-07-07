namespace DataAccess.Entities
{
    public class DeliveryTypes
    {
        protected DeliveryTypes() { }

        public DeliveryTypes(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public virtual void Update(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
    }
}
