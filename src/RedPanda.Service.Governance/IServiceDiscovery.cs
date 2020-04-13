using System.Threading.Tasks;

namespace RedPanda.Service.Governance
{
    public interface IServiceDiscovery
    {
        Task<string[]> GetServiceAddressesAsync(string serviceName, string serviceSpace = null, string serviceTag = null);
    }
}
