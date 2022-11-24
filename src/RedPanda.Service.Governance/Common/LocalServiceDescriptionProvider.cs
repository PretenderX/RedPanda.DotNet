namespace RedPanda.Service.Governance.Common
{
    public static class LocalServiceDescriptionProvider
    {
        public static ServiceDescription Build()
        {
            return new ServiceDescription
            {
                ServiceSpace = GetAppSetting(ServiceGovernanceConsts.ServiceSpace, false),
                ServiceName = GetAppSetting(ServiceGovernanceConsts.ServiceName, false),
                ServiceAliases = GetAppSetting(ServiceGovernanceConsts.ServiceAliases),
                ServiceSchema = GetAppSetting(ServiceGovernanceConsts.ServiceSchema, ServiceGovernanceConsts.DefaultServiceSchema),
                ServiceHost = GetAppSetting(ServiceGovernanceConsts.ServiceHost, false),
                ServicePort = GetAppSetting(ServiceGovernanceConsts.ServicePort, ServiceGovernanceConsts.DefaultServicePort),
                ServiceVirtualDirectory = GetAppSetting(ServiceGovernanceConsts.ServiceVirtualDirectory),
                ServiceHealthCheckRoute = GetAppSetting(ServiceGovernanceConsts.ServiceHealthRoute),

                ServiceCheckInterval = GetAppSetting(ServiceGovernanceConsts.ServiceCheckInterval, ServiceGovernanceConsts.DefaultServiceCheckInterval),
                ServiceCheckTimeout = GetAppSetting(ServiceGovernanceConsts.ServiceCheckTimeout, ServiceGovernanceConsts.DefaultServiceCheckTimeout),
                DeregisterCriticalServiceAfter = GetAppSetting(ServiceGovernanceConsts.DeregisterCriticalServiceAfter, ServiceGovernanceConsts.DefaultDeregisterCriticalServiceAfter),
            };
        }

        public static string GetLocalServiceSpace()
        {
            return GetAppSetting(ServiceGovernanceConsts.ServiceSpace);
        }

        private static string GetAppSetting(string name, bool isOptional = true) => ServiceGovernanceConfig.ServiceGovernanceConfigProvider.GetAppSetting(name, isOptional);
        private static T GetAppSetting<T>(string name, T defaultValue) => ServiceGovernanceConfig.ServiceGovernanceConfigProvider.GetAppSetting(name, defaultValue);
    }
}
