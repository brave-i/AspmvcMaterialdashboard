using System;

namespace Chatison.ViewModels.Admin.Group
{
    public class GroupListItemVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int TotalContacts { get; set; }
        public int TotalInvalid { get; set; }
        public int TotalOptOuts { get; set; }
    }
}
