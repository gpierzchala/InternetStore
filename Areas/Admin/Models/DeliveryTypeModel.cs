using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class DeliveryTypeModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Nazwa")]
        [MinLength(2, ErrorMessage = "Minimalna długość nazwy sposobu dostawy to 2 znaki")]
        [MaxLength(25, ErrorMessage = "Maksymalna długość nazwy sposobu dostawy to 25 znaków")]   
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$",ErrorMessage = "Podano błędną wartość")]
        [DisplayName("Koszt")]
        public string Price { get; set; }
    }
}