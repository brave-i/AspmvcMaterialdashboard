using AutoMapper;
using Chatison.Factory;
using Chatison.Infrastructure.DataLayer;
using Chatison.Infrastructure.Managers;
using Chatison.Infrastructure.Repositories;
using Chatison.ViewModels;
using Chatison.ViewModels.Admin.Group;
using System.Threading.Tasks;

namespace Chatison.Managers
{
    public class GroupManager : IGroupManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupManager(IUnitOfWork unitOfWork,
            IGroupRepository groupRepository,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _groupRepository = groupRepository;
            _mapper = mapper;

        }

        public async Task AddAsync(AddGroupVm model)
        {
            _groupRepository.Add(GroupFactory.CreateGroup(model));

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> IsExistsAsync(string name)
        {
            return await _groupRepository.IsExistsAsync(name);
        }

        public async Task<bool> IsExistsAsync(int id, string name)
        {
            return await _groupRepository.IsExistsAsync(id, name);
        }

        public async Task EditAsync(EditGroupVm model)
        {
            var group = _groupRepository.Find(model.Id);

            GroupFactory.CreateGroup(group, model);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<JqDataTableResponseVm<GroupListItemVm>> GetAsync(JqDataTableRequestVm model)
        {
            if (model.Length == 0)
            {
                model.Length = 10;
            }

            var filterKey = model.Search?.Value;

            var sortExpression = model.GetSortExpression();

            var response = await _groupRepository.GetAsync(filterKey, sortExpression, model.Start, model.Length);

            return _mapper.Map<JqDataTableResponseVm<GroupListItemVm>>(response);
        }

        public async Task DeleteAsync(int id)
        {
            _groupRepository.Delete(id);

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<string> GetNameAsync(int id)
        {
            return await _groupRepository.GetNameAsync(id);
        }
    }
}
