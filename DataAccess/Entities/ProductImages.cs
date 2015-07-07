namespace DataAccess.Entities
{
    public class ProductImages
    {
        protected ProductImages() {}

        public ProductImages(string imageName, byte[] imageBytes, Products product)
        {
            ImageName = imageName;
            ImageBytes = imageBytes;
            Product = product;
        }

        public virtual int ID { get; set; }
        public virtual string ImageName { get; set; }
        public virtual byte[] ImageBytes { get; set; }
        public virtual Products Product { get; set; }
    }
}
