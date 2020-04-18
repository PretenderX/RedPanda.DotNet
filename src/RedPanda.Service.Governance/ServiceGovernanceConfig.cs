using RedPanda.Service.Governance.Common;

namespace RedPanda.Service.Governance
{
    public static class ServiceGovernanceConfig
    {
        public static IServiceGovernanceConfigProvider ServiceGovernanceConfigProvider { get; set; } = new ServiceGovernanceConfigProvider();

        public static IJsonProvider JsonProvider { get; set; } = new SystemTextJsonProvider();
    }
}
