using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class CreateManufacturerModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        [StringLength(25,MinimumLength = 2,ErrorMessage = "Długość nazwy producenta nie może być mniejsza niż 2 znaki i większa niż 25")]
        public string Name { get; set; }
    }
}