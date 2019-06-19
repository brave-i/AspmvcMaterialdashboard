using Chatison.Factory;
using Chatison.Infrastructure.DataLayer;
using Chatison.Infrastructure.Managers;
using Chatison.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Chatison.Managers
{
    public class DataSeedManager : IDataSeedManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMobileProviderRepository _moblProviderRepository;

        public DataSeedManager(IUnitOfWork unitOfWork,
            IMobileProviderRepository moblProviderRepository)
        {
            _unitOfWork = unitOfWork;
            _moblProviderRepository = moblProviderRepository;
        }

        public async Task SeedMobileProvidersAsync()
        {
            if (await _moblProviderRepository.HasDataAsync())
            {
                return;
            }

            _moblProviderRepository.AddMany(MobileProviderFactory.CreateSeedProviders());

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
