using System.ComponentModel.DataAnnotations;

namespace Chatison.ViewModels.Admin.Contact
{
    public class ImportContactVm
    {
        [Required(ErrorMessage = "Please select column name")]
        public string FirstNameColumn { get; set; }

        [Required(ErrorMessage = "Please select column name")]
        public string LastNameColumn { get; set; }

        [Required(ErrorMessage = "Please select column name")]
        public string EmailColumn { get; set; }

        [Required(ErrorMessage = "Please select column name")]
        public string PhoneColumn { get; set; }
        
        public string ProviderColumn { get; set; }

        [Required(ErrorMessage = "Please select default group")]
        public int GroupId { get; set; }
        
        public string UploadedFileName { get; set; }
    }
}
