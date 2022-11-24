using Microsoft.Extensions.Configuration;

namespace RedPanda.Service.Governance.Configuration
{
    public static class ServiceDescriptionConfigurationExtension
    {
        public static ServiceDescription GetServiceDescription(this IConfiguration configuration, string sectionName = ServiceDescription.SectionName)
        {
            var serviceDescription = new ServiceDescription();

            configuration.Bind(sectionName, serviceDescription);

            return serviceDescription;
        }
    }
}
