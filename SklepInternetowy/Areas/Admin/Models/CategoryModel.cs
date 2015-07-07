using System.ComponentModel.DataAnnotations;

namespace SklepInternetowy.Areas.Admin.Models
{
    public class CategoryModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Nazwa kategorii musi być ciągiem o długości minimalnej 3 i maksymalnej 15.")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}