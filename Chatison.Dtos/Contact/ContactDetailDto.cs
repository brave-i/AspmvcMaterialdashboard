using System.Collections.Generic;

namespace Chatison.Dtos.Contact
{
    public class ContactDetailDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public IEnumerable<int> GroupIds { get; set; }
        public bool IsOptOut { get; set; }
        public int? ProviderId { get; set; }
        public bool IsAdmin { get; set; }
    }
}
