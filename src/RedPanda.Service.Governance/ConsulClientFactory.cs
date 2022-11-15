using Consul;
using System;

namespace RedPanda.Service.Governance
{
    public static class ConsulClientFactory
    {
        public static IConsulClient Create()
        {
            var consulAddress = ServiceGovernanceConfig.ServiceGovernanceConfigProvider.GetAppSetting(ServiceGovernanceConsts.ConsulAddress, false);
            var consulToken = ServiceGovernanceConfig.ServiceGovernanceConfigProvider.GetAppSetting(ServiceGovernanceConsts.ConsulToken);
            var consulClient = new ConsulClient(c =>
            {
                c.Address = new Uri(consulAddress);

                if (!string.IsNullOrEmpty(consulToken))
                {
                    c.Token = consulToken;
                }
            });

            return consulClient;
        }
    }
}
