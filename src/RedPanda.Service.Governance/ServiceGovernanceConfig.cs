using RedPanda.Service.Governance.Common;
using System;

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

        /// <summary>
        /// 默认60秒
        /// </summary>
        public static TimeSpan ServiceCheckInterval { get; set; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// 默认30秒
        /// </summary>
        public static TimeSpan ServiceCheckTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// 默认300秒
        /// </summary>
        public static TimeSpan DeregisterCriticalServiceAfter { get; set; } = TimeSpan.FromSeconds(300);
    }
}
