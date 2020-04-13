using System.Configuration;

namespace RedPanda.Service.Governance
{
    internal static class ServiceGovernanceConfigProvider
    {
        internal static string GetAppSetting(string name, bool isOptional = true)
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
