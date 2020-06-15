using Consul;
using RedPanda.Service.Governance.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance.Discovery
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        public async Task<CatalogService[]> GetCatalogServicesAsync(string serviceName, string serviceSpace = null, string serviceTag = null)
        {
            var scopedServiceName = GetScopedServiceName(serviceName, serviceSpace);

            using (var consulClient = ConsulClientFactory.Create())
            {
                var queryResult = string.IsNullOrEmpty(serviceTag) ? await consulClient.Catalog.Service(scopedServiceName) : await consulClient.Catalog.Service(scopedServiceName, serviceTag);

                return queryResult.Response;
            }
        }

        public Task<CatalogService[]> GetCatalogServicesByLocalSpaceAsync(string serviceName, string serviceTag = null)
        {
            var localServiceSpace = LocalServiceDescriptionProvider.GetLocalServiceSpace();

            return GetCatalogServicesAsync(serviceName, localServiceSpace, serviceTag);
        }

        public async Task<string[]> GetServiceAddressesAsync(string serviceName, string serviceSpace = null, string serviceTag = null)
        {
            var catalogServices = await GetCatalogServicesAsync(serviceName, serviceSpace, serviceTag);

            return catalogServices.Select(s => s.GetRegisteredServiceAddress()).ToArray();
        }

        public Task<string[]> GetServiceAddressesByLocalSpaceAsync(string serviceName, string serviceTag = null)
        {
            var localServiceSpace = LocalServiceDescriptionProvider.GetLocalServiceSpace();

            return GetServiceAddressesAsync(serviceName, localServiceSpace, serviceTag);
        }

        private string GetScopedServiceName(string serviceName, string serviceSpace)
        {
            return string.IsNullOrEmpty(serviceSpace) ? serviceName : $"{serviceSpace}.{serviceName}";
        }
    }
}
