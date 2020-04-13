using Consul;
using System;

namespace RedPanda.Service.Governance
{
    public static class ConsulClientFactory
    {
        public static IConsulClient Create()
        {
            var consulAddress = ServiceGovernanceConfigProvider.GetAppSetting(ServiceGovernanceConsts.ConsulAddress, false);
            var consulClient = new ConsulClient(c => c.Address = new Uri(consulAddress));

            return consulClient;
        }
    }
}
