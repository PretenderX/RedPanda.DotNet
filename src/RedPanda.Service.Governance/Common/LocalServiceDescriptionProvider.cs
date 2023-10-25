namespace RedPanda.Service.Governance.Common
{
    public static class LocalServiceDescriptionProvider
    {
        private static ServiceDescription value;
        public static ServiceDescription Value => value ?? (value = Build());

        public static void RefreshValue()
        {
            value = null;
        }

        public static ServiceDescription Build()
        {
            return new ServiceDescription
            {
                ServiceSpace = GetAppSetting(ServiceGovernanceConsts.ServiceSpace, false),
                ServiceName = GetAppSetting(ServiceGovernanceConsts.ServiceName, false),
                FullServiceNameFormat = GetFullServiceNameFormat(),
                ServiceAliases = GetAppSetting(ServiceGovernanceConsts.ServiceAliases),
                ServiceSchema = GetAppSetting(ServiceGovernanceConsts.ServiceSchema, ServiceGovernanceConsts.DefaultServiceSchema),
                ServiceHost = GetAppSetting(ServiceGovernanceConsts.ServiceHost, false),
                ServicePort = GetAppSetting(ServiceGovernanceConsts.ServicePort, ServiceGovernanceConsts.DefaultServicePort),
                ServiceVirtualDirectory = GetAppSetting(ServiceGovernanceConsts.ServiceVirtualDirectory),
                ServiceHealthCheckRoute = GetAppSetting(ServiceGovernanceConsts.ServiceHealthRoute),

                ServiceCheckInterval = GetAppSetting(ServiceGovernanceConsts.ServiceCheckInterval, ServiceGovernanceConsts.DefaultServiceCheckInterval),
                ServiceCheckTimeout = GetAppSetting(ServiceGovernanceConsts.ServiceCheckTimeout, ServiceGovernanceConsts.DefaultServiceCheckTimeout),
                DeregisterCriticalServiceAfter = GetAppSetting<int?>(ServiceGovernanceConsts.DeregisterCriticalServiceAfter, default),
            };
        }

        public static string GetLocalServiceSpace()
        {
            return GetAppSetting(ServiceGovernanceConsts.ServiceSpace);
        }

        public static string GetFullServiceNameFormat()
        {
            return GetAppSetting(nameof(ServiceDescription.FullServiceNameFormat), ServiceGovernanceConsts.DefaultFullServiceNameFormat);
        }

        private static string GetAppSetting(string name, bool isOptional = true) => ServiceGovernanceConfig.ServiceGovernanceConfigProvider.GetAppSetting(name, isOptional);
        private static T GetAppSetting<T>(string name, T defaultValue) => ServiceGovernanceConfig.ServiceGovernanceConfigProvider.GetAppSetting(name, defaultValue);
    }
}
