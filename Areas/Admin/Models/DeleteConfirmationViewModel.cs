namespace SklepInternetowy.Areas.Admin.Models
{
    public class DeleteConfirmationViewModel
    {
        public string PostDeleteAction { get; set; }
        public string PostDeleteController { get; set; }
        public int DeleteEntityId { get; set; }
        public string HeaderText { get; set; }
    }
}