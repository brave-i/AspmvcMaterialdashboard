using Chatison.Dtos;
using Chatison.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatison.Infrastructure.Repositories
{
    public interface IMobileProviderRepository
    {
        Task<bool> HasDataAsync();
        void AddMany(IEnumerable<MobileProvider> entities);
        IEnumerable<SelectListItemDto> GetSelectListItems();
    }
}
