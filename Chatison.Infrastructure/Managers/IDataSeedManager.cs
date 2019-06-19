using System.Threading.Tasks;

namespace Chatison.Infrastructure.Managers
{
    public interface IDataSeedManager
    {
        Task SeedMobileProvidersAsync();
    }
}
