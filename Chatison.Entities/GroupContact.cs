using System;

namespace Chatison.Entities
{
    public class GroupContact
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Group Group { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
