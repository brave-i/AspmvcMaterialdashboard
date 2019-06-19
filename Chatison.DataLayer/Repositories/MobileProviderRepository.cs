using Chatison.Dtos;
using Chatison.Entities;
using Chatison.Infrastructure.Repositories;
using Chatison.Utilities;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Chatison.DataLayer.Repositories
{
    public class MobileProviderRepository : IMobileProviderRepository
    {
        private readonly DataContext _dataContext;

        public MobileProviderRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> HasDataAsync()
        {
            return await _dataContext.MobileProviders.AnyAsync();
        }

        public void AddMany(IEnumerable<MobileProvider> entities)
        {
            foreach (var entity in entities)
            {
                _dataContext.MobileProviders.Add(entity);
            }
        }

        public IEnumerable<SelectListItemDto> GetSelectListItems()
        {
            return _dataContext.MobileProviders
                .Where(x => x.Status == Constants.RecordStatus.Active).Select(x =>
                new SelectListItemDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }
    }
}
