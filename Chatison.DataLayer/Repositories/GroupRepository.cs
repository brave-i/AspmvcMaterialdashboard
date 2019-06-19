using Chatison.Dtos;
using Chatison.Dtos.Group;
using Chatison.Entities;
using Chatison.Infrastructure.Repositories;
using Chatison.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Constants = Chatison.Utilities.Constants;

namespace Chatison.DataLayer.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly DataContext _dataContext;

        public GroupRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Group entity)
        {
            _dataContext.Groups.Add(entity);
        }

        public Group Find(int id)
        {
            return _dataContext.Groups.Find(id);
        }

        public async Task<bool> IsExistsAsync(string name)
        {
            return await _dataContext.Groups.AnyAsync(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                && x.Status != Constants.RecordStatus.Deleted);
        }

        public async Task<bool> IsExistsAsync(int id, string name)
        {
            return await _dataContext.Groups.AnyAsync(x => x.Id != id
                && x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                && x.Status != Constants.RecordStatus.Deleted);
        }

        public async Task<PagedResultDto<GroupListItemDto>> GetAsync(string filterKey, string sortExpression,
            int offset, int limit)
        {
            if (limit == 0)
            {
                limit = 10;
            }
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dataContext));

            var linqStmt = from g in _dataContext.Groups
                           where g.Status != Constants.RecordStatus.Deleted
                                 && (filterKey == null || g.Name.Contains(filterKey))
                           let contacts = (from c in g.GroupContacts.Select(x => x.Contact)
                                           join u in manager.Users on c.UserId equals u.Id
                                           where u.Status != Constants.RecordStatus.Deleted
                                           select c)
                           let totalContacts = contacts.Count()
                           let totalOptOuts = contacts.Count(x => x.IsOptOut)
                           select new GroupListItemDto
                           {
                               Id = g.Id,
                               Name = g.Name,
                               CreatedAt = g.CreatedAt,
                               TotalContacts = totalContacts,
                               TotalOptOuts = totalOptOuts,
                               TotalInvalid = 0
                           };

            var pagedResult = new PagedResultDto<GroupListItemDto>
            {
                TotalRecords = await _dataContext.Groups.CountAsync(),
                TotalRecordsFiltered = await linqStmt.CountAsync(),
                ResultSet = await linqStmt.OrderBy(sortExpression)
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync()
            };

            return pagedResult;
        }

        public void Delete(int id)
        {
            var group = _dataContext.Groups.Find(id);

            group.UpdatedAt = Utility.GetDateTime();
            group.Status = Constants.RecordStatus.Deleted;
        }

        public IEnumerable<SelectListItemDto> GetSelectListItems()
        {
            return _dataContext.Groups
                .Where(x => x.Status == Constants.RecordStatus.Active).Select(x =>
                    new SelectListItemDto
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();
        }

        public async Task<string> GetNameAsync(int id)
        {
            return await _dataContext.Groups.Where(x => x.Id == id).Select(x => x.Name).SingleOrDefaultAsync();
        }
    }
}
