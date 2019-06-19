using Chatison.Utilities;
using System;
using System.Collections.Generic;

namespace Chatison.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public string HasKeyword { get; set; }
        public int? OraganizationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Constants.RecordStatus Status { get; set; }

        public virtual ICollection<GroupContact> GroupContacts { get; set; }
    }
}
