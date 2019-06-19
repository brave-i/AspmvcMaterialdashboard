using Chatison.ViewModels;
using Chatison.ViewModels.Admin.Group;
using System.Threading.Tasks;

namespace Chatison.Infrastructure.Managers
{
    public interface IGroupManager
    {
        Task AddAsync(AddGroupVm model);
        Task<bool> IsExistsAsync(int id, string name);
        Task<bool> IsExistsAsync(string name);
        Task EditAsync(EditGroupVm model);
        Task<JqDataTableResponseVm<GroupListItemVm>> GetAsync(JqDataTableRequestVm model);
        Task DeleteAsync(int id);
        Task<string> GetNameAsync(int id);
    }
}
