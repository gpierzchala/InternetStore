using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Imię")]
        [StringLength(20, ErrorMessage = "Ilośc znaków musi być w przedziale {1} - {2}.", MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Nazwisko")]
        [StringLength(20, ErrorMessage = "Ilośc znaków musi być w przedziale {1} - {2}.", MinimumLength = 2)]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Miasto")]
        [StringLength(100, ErrorMessage = "Ilośc znaków musi być w przedziale {1} - {2}.", MinimumLength = 2)]
        public string City { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Adres")]
        [StringLength(100, ErrorMessage = "Ilośc znaków musi być w przedziale {1} - {2}.", MinimumLength = 2)]
        public string Address { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}", ErrorMessage = "Podany kod pocztowy jest niepoprawny. Prawidłowy format to xx-xxx")]
        public string ZipCode { get; set; }
    }
}