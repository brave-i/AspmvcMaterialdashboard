using System;

namespace Chatison.Dtos.Group
{
    public class GroupListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalContacts { get; set; }
        public int TotalInvalid { get; set; }
        public int TotalOptOuts { get; set; }
    }
}
