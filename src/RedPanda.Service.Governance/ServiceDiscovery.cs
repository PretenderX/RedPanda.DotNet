using Consul;
using System.Linq;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance
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

        public async Task<string[]> GetServiceAddressesAsync(string serviceName, string serviceSpace = null, string serviceTag = null)
        {
            var catalogServices = await GetCatalogServicesAsync(serviceName, serviceSpace, serviceTag);

            return catalogServices.Select(s =>
            {
                s.ServiceMeta.TryGetValue(ServiceGovernanceConsts.ServiceSchema, out var serviceSchema);
                serviceSchema = serviceSchema ?? "http";

                if (s.ServiceMeta.TryGetValue(ServiceGovernanceConsts.ServiceVirtualDirectory, out var virtualDirecotory))
                {
                    if (!string.IsNullOrEmpty(virtualDirecotory))
                    {
                        return $"{serviceSchema}://{s.ServiceAddress}:{s.ServicePort}/{virtualDirecotory}";
                    }
                }

                return $"{serviceSchema}://{s.ServiceAddress}:{s.ServicePort}";
            }).ToArray();
        }

        private string GetScopedServiceName(string serviceName, string serviceSpace)
        {
            return string.IsNullOrEmpty(serviceSpace) ? serviceName : $"{serviceSpace}.{serviceName}";
        }
    }
}
