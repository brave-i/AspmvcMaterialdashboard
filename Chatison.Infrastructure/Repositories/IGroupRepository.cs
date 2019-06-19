using Chatison.Dtos;
using Chatison.Dtos.Group;
using Chatison.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatison.Infrastructure.Repositories
{
    public interface IGroupRepository
    {
        Task<bool> IsExistsAsync(string name);
        Task<bool> IsExistsAsync(int id, string name);
        void Add(Group entity);
        Group Find(int id);
        Task<PagedResultDto<GroupListItemDto>> GetAsync(string filterKey, string sortExpression, int offset, int limit);
        void Delete(int id);
        IEnumerable<SelectListItemDto> GetSelectListItems();
        Task<string> GetNameAsync(int id);
    }
}
