using Autofac;

namespace RedPanda.Service.Governance.Autofac
{
    public static class ContainerBuilderExtensionOfServiceGovernance
    {
        public static ContainerBuilder UseServiceGovernance(this ContainerBuilder builder)
        {
            builder.RegisterType<ServiceRegistration>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ServiceDiscovery>().AsImplementedInterfaces().SingleInstance();

            return builder;
        }
    }
}