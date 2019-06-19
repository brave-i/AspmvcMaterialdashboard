using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chatison.ViewModels.Admin.Contact
{
    public class EditContactVm
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter first name.")]
        [MaxLength(250)]
        public string FirstName { get; set; }

        [MaxLength(250)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [MaxLength(250)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter phone number.")]
        public string Phone { get; set; }

        public List<int> GroupIds { get; set; }
        public bool IsOptOut { get; set; }
        public int? ProviderId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
