using Chatison.ViewModels;
using System.Collections.Generic;

namespace Chatison.Infrastructure.Managers
{
    public interface ISelectListManager
    {
        IEnumerable<SelectListItemVm> GetMobileProviders();
        IEnumerable<SelectListItemVm> GetGeoups();
    }
}
