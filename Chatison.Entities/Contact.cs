using Chatison.Utilities;
using System;
using System.Collections.Generic;

namespace Chatison.Entities
{
    public class Contact
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool ReceiveSms { get; set; }
        public bool ReceivedFirstSms { get; set; }
        public int? OrganizationId { get; set; }
        public int? MobileProviderId { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool OptInSms { get; set; }
        public bool OptInEmail { get; set; }
        public bool IsOptOut { get; set; }
        public Constants.ContactSource Source { get; set; }

        public virtual ICollection<GroupContact> GroupContacts { get; set; }
    }
}
