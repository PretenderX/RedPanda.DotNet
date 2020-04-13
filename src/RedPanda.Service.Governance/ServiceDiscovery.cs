using System.Linq;
using System.Threading.Tasks;

namespace RedPanda.Service.Governance
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        public async Task<string[]> GetServiceAddressesAsync(string serviceName, string serviceSpace = null, string serviceTag = null)
        {
            using (var consulClient = ConsulClientFactory.Create())
            {
                var serviceFullName = GetServiceFullName(serviceName, serviceSpace);
                var categoryService = string.IsNullOrEmpty(serviceTag) ? await consulClient.Catalog.Service(serviceFullName) : await consulClient.Catalog.Service(serviceFullName, serviceTag);
                var service = categoryService.Response.FirstOrDefault();

                return categoryService.Response.Select(s => $"{s.ServiceAddress}:{s.ServicePort}").ToArray();
            }
        }

        private string GetServiceFullName(string serviceName, string serviceSpace)
        {
            return string.IsNullOrEmpty(serviceSpace) ? serviceName : $"{serviceSpace}.{serviceName}";
        }
    }
}
