using AutoMapper;
using Chatison.Factory;
using Chatison.Infrastructure.DataLayer;
using Chatison.Infrastructure.Managers;
using Chatison.Infrastructure.Repositories;
using Chatison.ViewModels;
using Chatison.ViewModels.Admin.Contact;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chatison.Managers
{
    public class ContactManager : IContactManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactManager(IUnitOfWork unitOfWork,
            IContactRepository contactRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(AddContactVm model, string userId)
        {
            _contactRepository.Add(ContactFactory.CreateContact(model, userId));

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<JqDataTableResponseVm<ContactListItemVm>> GetAsync(JqDataTableRequestVm model, int? groupId)
        {
            if (model.Length == 0)
            {
                model.Length = 10;
            }

            var filterKey = model.Search?.Value;

            var sortExpression = model.GetSortExpression();

            var response = await _contactRepository.GetAsync(groupId, filterKey, sortExpression, model.Start, model.Length);

            return _mapper.Map<JqDataTableResponseVm<ContactListItemVm>>(response);
        }

        public async Task<EditContactVm> GetForEditAsync(string id)
        {
            var contact = await _contactRepository.GetForEditAsync(id);
            return _mapper.Map<EditContactVm>(contact);
        }

        public async Task EditAsync(EditContactVm model)
        {
            var contact = await _contactRepository.FindAsync(model.Id);

            var removedGroups = ContactFactory.CreateContact(contact, model);

            await _unitOfWork.SaveChangesAsync();

            if (removedGroups != null && removedGroups.Any())
            {
                await _contactRepository.RemoveContactGroupsAsync(contact.UserId, removedGroups);

                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ContactListItemVm>> GetAllAsync(int? groupId)
        {
            var items = await _contactRepository.GetAllAsync(groupId);
            return _mapper.Map<IEnumerable<ContactListItemVm>>(items);
        }
    }
}
