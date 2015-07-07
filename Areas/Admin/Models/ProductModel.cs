using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [StringLength(15,MinimumLength = 3, ErrorMessage = "Nazwa produktu musi być dłuższa niż 3 znaków i krótsza niż 15.")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public string Price { get; set; }
        public string Category { get; set; }
        public string Manufacturer { get; set; }
        [Required]
        public string Quantity { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsRecent { get; set; }
        public bool IsBestSeller { get; set; }
        public string ShortDescription { get; set; }
    }
}