using Chatison.Entities;
using Chatison.Utilities;
using Chatison.ViewModels.Admin.Group;

namespace Chatison.Factory
{
    public class GroupFactory
    {
        public static Group CreateGroup(AddGroupVm model)
        {
            return new Group
            {
                Name = model.Name,
                CreatedAt = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active
            };
        }

        public static void CreateGroup(Group entity, EditGroupVm model)
        {
            entity.Name = model.Name;
            entity.UpdatedAt = Utility.GetDateTime();
        }
    }
}
