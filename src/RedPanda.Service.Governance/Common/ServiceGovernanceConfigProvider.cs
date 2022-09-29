using Microsoft.Extensions.Configuration;
using RedPanda.Service.Governance.Exceptions;

namespace RedPanda.Service.Governance.Common
{
    public class ServiceGovernanceConfigProvider : IServiceGovernanceConfigProvider
    {
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
    }
}
