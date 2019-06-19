using System.ComponentModel.DataAnnotations;

namespace Chatison.ViewModels.Admin.Group
{
    public class EditGroupVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter group name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
