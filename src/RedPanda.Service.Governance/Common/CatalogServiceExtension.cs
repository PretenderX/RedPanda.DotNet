using Consul;

namespace RedPanda.Service.Governance.Common
{
    public static class CatalogServiceExtension
    {
        public static string GetRegisteredServiceAddress(this CatalogService catalogService)
        {
            if (!catalogService.ServiceMeta.TryGetValue(ServiceGovernanceConsts.ServiceSchema, out var serviceSchema) ||
                string.IsNullOrEmpty(serviceSchema))
            {
                serviceSchema = ServiceGovernanceConsts.DefaultServiceSchema;
            }

            var address = $"{serviceSchema}://{catalogService.ServiceAddress}";
            var port = catalogService.ServicePort == default ? ServiceGovernanceConsts.DefaultServicePort : catalogService.ServicePort;

            if (port > 0 && port != 80 && port != 443)
            {
                address = $"{address}:{port}";
            }

            if (catalogService.ServiceMeta.TryGetValue(ServiceGovernanceConsts.ServiceVirtualDirectory, out var virtualDirectory))
            {
                virtualDirectory = virtualDirectory?.Trim('/');

                if (!string.IsNullOrEmpty(virtualDirectory))
                {
                    address = $"{address}/{virtualDirectory.Trim('/')}";
                }
            }

            return address;
        }
    }
}
