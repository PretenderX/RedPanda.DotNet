using RedPanda.Service.Governance.Exceptions;
using System.Configuration;

namespace RedPanda.Service.Governance.Common
{
    public class ServiceGovernanceConfigProvider : IServiceGovernanceConfigProvider
    {
        public string GetAppSetting(string name, bool isOptional = true)
        {
            var value = ConfigurationManager.AppSettings.Get(name);

            if (!isOptional && string.IsNullOrEmpty(value))
            {
                throw new NotFoundServiceGovernanceConfigItemException(name);
            }

            return value;
        }
    }
}
