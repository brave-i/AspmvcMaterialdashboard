using Chatison.ViewModels;
using Chatison.ViewModels.Admin.Contact;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatison.Infrastructure.Managers
{
    public interface IContactManager
    {
        Task AddAsync(AddContactVm model, string userId);
        Task<JqDataTableResponseVm<ContactListItemVm>> GetAsync(JqDataTableRequestVm model, int? groupId);
        Task<EditContactVm> GetForEditAsync(string id);
        Task EditAsync(EditContactVm model);
        Task<IEnumerable<ContactListItemVm>> GetAllAsync(int? groupId);
    }
}
