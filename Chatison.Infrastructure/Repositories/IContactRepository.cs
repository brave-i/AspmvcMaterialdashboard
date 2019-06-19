using Chatison.Dtos;
using Chatison.Dtos.Contact;
using Chatison.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatison.Infrastructure.Repositories
{
    public interface IContactRepository
    {
        void Add(Contact entity);
        Task<Contact> FindAsync(string id);
        Task<PagedResultDto<ContactListItemDto>> GetAsync(int? groupId, string filterKey, string sortExpression, int offset,
            int limit);
        Task<ContactDetailDto> GetForEditAsync(string id);
        Task<IEnumerable<ContactListItemDto>> GetAllAsync(int? groupId = null);
        Task RemoveContactGroupsAsync(string contactId, List<int> ids);
    }
}
