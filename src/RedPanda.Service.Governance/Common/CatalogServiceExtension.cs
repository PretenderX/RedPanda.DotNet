using Consul;

namespace RedPanda.Service.Governance.Common
{
    public static class CatalogServiceExtension
    {
        public static string GetRegisteredServiceAddress(this CatalogService catalogService)
        {
            catalogService.ServiceMeta.TryGetValue(ServiceGovernanceConsts.ServiceSchema, out var serviceSchema);
            serviceSchema = serviceSchema ?? "http";

            if (catalogService.ServiceMeta.TryGetValue(ServiceGovernanceConsts.ServiceVirtualDirectory, out var virtualDirecotory))
            {
                if (!string.IsNullOrEmpty(virtualDirecotory))
                {
                    return $"{serviceSchema}://{catalogService.ServiceAddress}:{catalogService.ServicePort}/{virtualDirecotory}";
                }
            }

            return $"{serviceSchema}://{catalogService.ServiceAddress}:{catalogService.ServicePort}";
        }
    }
}
