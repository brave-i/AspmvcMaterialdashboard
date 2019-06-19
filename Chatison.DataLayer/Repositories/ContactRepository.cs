using Chatison.Dtos;
using Chatison.Dtos.Contact;
using Chatison.Entities;
using Chatison.Infrastructure.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace Chatison.DataLayer.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly DataContext _dataContext;

        public ContactRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Contact entity)
        {
            _dataContext.Contacts.Add(entity);
        }

        public async Task<Contact> FindAsync(string id)
        {
            return await _dataContext.Contacts.Include(x => x.GroupContacts).SingleOrDefaultAsync(x => x.UserId.Equals(id));
        }

        public async Task<PagedResultDto<ContactListItemDto>> GetAsync(int? groupId, string filterKey, string sortExpression, int offset, int limit)
        {
            if (limit == 0)
            {
                limit = 10;
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dataContext));

            var linqStmt = from c in _dataContext.Contacts
                           join u in manager.Users
                               on c.UserId equals u.Id
                           where u.Status != Utilities.Constants.RecordStatus.Deleted
                                 && (groupId == null || c.GroupContacts.Any(x => x.GroupId == groupId.Value))
                                 && (filterKey == null || c.FirstName.Contains(filterKey)
                                                           || u.Email.Contains(filterKey)
                                                           || u.PhoneNumber.Contains(filterKey))
                           select new ContactListItemDto
                           {
                               Id = c.UserId,
                               FirstName = c.FirstName,
                               LastName = c.LastName,
                               Email = u.Email,
                               Phone = u.PhoneNumber,
                               CreatedAt = c.CreatedAt,
                               Source = c.Source,
                               IsOptout = c.IsOptOut
                           };

            var pagedResult = new PagedResultDto<ContactListItemDto>
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

        public async Task<ContactDetailDto> GetForEditAsync(string id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dataContext));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dataContext));
            var adminRoleId = (await roleManager.Roles.SingleAsync(x => x.Name.Equals(Utilities.Constants.UserRoles.Admin))).Id;

            var contact = await (from c in _dataContext.Contacts
                                 join u in userManager.Users
                                     on c.UserId equals u.Id
                                 where c.UserId.Equals(id)
                                 select new ContactDetailDto
                                 {
                                     Id = c.UserId,
                                     FirstName = c.FirstName,
                                     LastName = c.LastName,
                                     Email = u.Email,
                                     Phone = u.PhoneNumber,
                                     IsOptOut = c.IsOptOut,
                                     ProviderId = c.MobileProviderId,
                                     IsAdmin = u.Roles.Any(x => x.RoleId.Equals(adminRoleId)),
                                     GroupIds = c.GroupContacts.Select(x => x.GroupId)
                                 }).SingleOrDefaultAsync();
            return contact;
        }

        public async Task<IEnumerable<ContactListItemDto>> GetAllAsync(int? groupId = null)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_dataContext));

            var linqStmt = from c in _dataContext.Contacts
                           join u in manager.Users
                               on c.UserId equals u.Id
                           where u.Status != Utilities.Constants.RecordStatus.Deleted
                                 && (groupId == null || c.GroupContacts.Any(x => x.GroupId == groupId.Value))
                           select new ContactListItemDto
                           {
                               Id = c.UserId,
                               FirstName = c.FirstName,
                               LastName = c.LastName,
                               Email = u.Email,
                               Phone = u.PhoneNumber,
                               CreatedAt = c.CreatedAt,
                               Source = Utilities.Constants.ContactSource.Manually,
                               IsOptout = c.IsOptOut,
                               GroupNames = c.GroupContacts.Select(x => x.Group.Name)
                           };

            return await linqStmt.ToListAsync();
        }

        public async Task RemoveContactGroupsAsync(string contactId, List<int> ids)
        {
            foreach (var id in ids)
            {
                var groupContact = await _dataContext.GroupContacts.SingleAsync(x => x.UserId.Equals(contactId) && x.GroupId == id);
                _dataContext.GroupContacts.Remove(groupContact);
            }
        }
    }
}
