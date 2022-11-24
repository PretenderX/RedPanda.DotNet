using Consul;

namespace RedPanda.Service.Governance.Common
{
    public static class CatalogServiceExtension
    {
        public static string GetRegisteredServiceAddress(this CatalogService catalogService)
        {
            catalogService.ServiceMeta.TryGetValue(ServiceGovernanceConsts.ServiceSchema, out var serviceSchema);
            serviceSchema = serviceSchema ?? "http";

            var address = $"{serviceSchema}://{catalogService.ServiceAddress}:{catalogService.ServicePort}";

            if (catalogService.ServiceMeta.TryGetValue(ServiceGovernanceConsts.ServiceVirtualDirectory, out var virtualDirecotory) &&
                !string.IsNullOrEmpty(virtualDirecotory))
            {
                address = $"{address}/{virtualDirecotory}";
            }

            return address;
        }
    }
}
