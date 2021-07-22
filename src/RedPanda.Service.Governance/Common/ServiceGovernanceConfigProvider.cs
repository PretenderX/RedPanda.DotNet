#if NETSTANDARD
using System.Configuration;
#endif
#if NETCOREAPP3_0 || NET5_0
using Microsoft.Extensions.Configuration;
#endif
using RedPanda.Service.Governance.Exceptions;

namespace RedPanda.Service.Governance.Common
{
    public class ServiceGovernanceConfigProvider : IServiceGovernanceConfigProvider
    {
#if NETSTANDARD2_0
        public string GetAppSetting(string name, bool isOptional = true)
        {
            var value = ConfigurationManager.AppSettings.Get(name);

            if (!isOptional && string.IsNullOrEmpty(value))
            {
                throw new NotFoundServiceGovernanceConfigItemException(name);
            }

            return value;
        }
#endif

#if NETCOREAPP3_0 || NET5_0
        protected readonly IConfiguration Configuration;

        public ServiceGovernanceConfigProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual string GetAppSetting(string name, bool isOptional = true)
        {
            var value = Configuration[name];

            if (!isOptional && string.IsNullOrEmpty(value))
            {
                throw new NotFoundServiceGovernanceConfigItemException(name);
            }

            return value;
        }
#endif
    }
}
