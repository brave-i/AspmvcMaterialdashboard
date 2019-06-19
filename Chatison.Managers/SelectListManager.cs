using AutoMapper;
using Chatison.Infrastructure.Managers;
using Chatison.Infrastructure.Repositories;
using Chatison.ViewModels;
using System.Collections.Generic;

namespace Chatison.Managers
{
    public class SelectListManager : ISelectListManager
    {
        private readonly IMobileProviderRepository _mobileProviderRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public SelectListManager(IMobileProviderRepository mobileProviderRepository,
            IGroupRepository groupRepository,
            IMapper mapper)
        {
            _mobileProviderRepository = mobileProviderRepository;
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public IEnumerable<SelectListItemVm> GetMobileProviders()
        {
            var items = _mobileProviderRepository.GetSelectListItems();
            return _mapper.Map<IEnumerable<SelectListItemVm>>(items);
        }

        public IEnumerable<SelectListItemVm> GetGeoups()
        {
            var items = _groupRepository.GetSelectListItems();
            return _mapper.Map<IEnumerable<SelectListItemVm>>(items);
        }
    }
}
