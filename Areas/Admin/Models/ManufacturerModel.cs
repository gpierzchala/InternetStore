using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class ManufacturerModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Nazwa producenta nie może być krótsza niż 3 i dłuższa niż 15 znaków.")]
        public string Name { get; set; }
    }
}