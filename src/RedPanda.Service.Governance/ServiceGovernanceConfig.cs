using RedPanda.Service.Governance.Common;

namespace RedPanda.Service.Governance
{
    public static class ServiceGovernanceConfig
    {
#if NETSTANDARD2_0
        public static IServiceGovernanceConfigProvider ServiceGovernanceConfigProvider { get; set; } = new ServiceGovernanceConfigProvider();
#endif
#if NETCOREAPP3_0
        public static IServiceGovernanceConfigProvider ServiceGovernanceConfigProvider { get; set; }
#endif

        public static IJsonProvider JsonProvider { get; set; } = new SystemTextJsonProvider();
    }
}
