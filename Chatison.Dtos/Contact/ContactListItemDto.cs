using Chatison.Utilities;
using System;
using System.Collections.Generic;

namespace Chatison.Dtos.Contact
{
    public class ContactListItemDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public Constants.ContactSource Source { get; set; }
        public bool IsOptout { get; set; }
        public IEnumerable<string> GroupNames { get; set; }
    }
}
