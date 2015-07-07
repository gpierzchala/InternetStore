using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace SklepInternetowy.Models
{
    public class RegisterModel
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
        [DisplayName("Hasło")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Ilośc znaków musi być w przedziale {1} - {2}.", MinimumLength = 2)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Powtórz hasło")]
        [Compare("Password", ErrorMessage = "Podane hasła są różne")]
        [DataType(DataType.Password)]
        public string RepeatedPassword { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Miasto")]
        [StringLength(100, ErrorMessage = "Ilośc znaków musi być w przedziale {1} - {2}.", MinimumLength = 2)]
        public string City { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Ulica")]
        [StringLength(100, ErrorMessage = "Ilośc znaków musi być w przedziale {1} - {2}.", MinimumLength = 2)]
        public string Street { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Numer budynku")]
        [Integer(ErrorMessage = "Numer budynku powinien być wartością liczbową")]
        public string HouseNumber { get; set; }

        [DisplayName("Numer mieszkania")]
        [Integer(ErrorMessage = "Numer mieszkania powinien być wartością liczbową")]
        public string FlatNumber { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Kod pocztowy")]
        [RegularExpression(@"[0-9]{2}-[0-9]{3}",
            ErrorMessage = "Podany kod pocztowy jest niepoprawny. Prawidłowy format to xx-xxx")]
        public string ZipCode { get; set; }
    }
}