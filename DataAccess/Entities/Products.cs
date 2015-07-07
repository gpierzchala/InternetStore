namespace DataAccess.Entities
{
    public class Products
    {
        protected Products()
        {
        }

        public Products(
            string name,
            string description,
            decimal price,
            Categories category,
            Manufacturers manufacturer,
            int quantity,
            bool isFeatured,
            bool isRecent,
            bool isBestSeller,
            string shortDescription)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            Manufacturer = manufacturer;
            Quantity = quantity;
            IsFeatured = isFeatured;
            IsRecent = isRecent;
            IsBestSeller = isBestSeller;
            ShortDescription = shortDescription;
        }

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal Price { get; set; }
        public virtual int Quantity { get; set; }
        public virtual Categories Category { get; set; }
        public virtual Manufacturers Manufacturer { get; set; }
        public virtual bool IsRecent { get; set; }
        public virtual bool IsFeatured { get; set; }
        public virtual bool IsBestSeller { get; set; }
        public virtual string ShortDescription { get; set; }

        public virtual void ChangeDetails(
            string name, string description, decimal price, int quantity,
            bool isFeatured,
            bool isRecent,
            string shortDescription)
        {
            if (name != null && Name != name)
            {
                Name = name;
            }
            if (Description != description || ShortDescription != shortDescription)
            {
                Description = description;
                ShortDescription = shortDescription;
            }
            if (Price != price)
            {
                Price = price;
            }
            if (Quantity != quantity)
            {
                Quantity = quantity;
            }
            if (IsFeatured != isFeatured || IsRecent != isRecent)
            {
                IsFeatured = isFeatured;
                IsRecent = isRecent;
            }
        }

        public virtual void ChangeCategory(Categories category)
        {
            if (category != null && Category != category)
            {
                Category = category;
            }
        }

        public virtual void ChangeManufacturer(Manufacturers manufacturer)
        {
            if (manufacturer != null && Manufacturer != manufacturer)
            {
                Manufacturer = manufacturer;
            }
        }
    }
}