using System;

namespace RedPanda.Service.Governance
{
    public static class SelfServiceDescriptionProvider
    {
        public static ServiceDescription Build()
        {
            return new ServiceDescription
            {
                ServiceSpace = GetAppSetting(ServiceGovernanceConsts.ServiceSpace),
                ServiceName = GetAppSetting(ServiceGovernanceConsts.ServiceName, false),
                ServiceAliases = GetAppSetting(ServiceGovernanceConsts.ServiceAliases),
                ServiceSchema = GetAppSetting(ServiceGovernanceConsts.ServiceSchema),
                Host = GetAppSetting(ServiceGovernanceConsts.ServiceHost, false),
                Port = Convert.ToInt32(GetAppSetting(ServiceGovernanceConsts.ServicePort, false)),
                HealthCheckRoute = GetAppSetting(ServiceGovernanceConsts.ServiceHealthRoute),
            };
        }

        private static string GetAppSetting(string name, bool isOptional = true) => ServiceGovernanceConfigProvider.GetAppSetting(name, isOptional);
    }
}
