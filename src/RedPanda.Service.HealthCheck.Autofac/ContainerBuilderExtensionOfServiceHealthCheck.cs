using Autofac;
using System.Reflection;

namespace RedPanda.Service.HealthCheck.Autofac
{
    public static class ContainerBuilderExtensionOfServiceHealthCheck
    {
        public static ContainerBuilder UseServiceHealthCheck(this ContainerBuilder builder,
            HealthCheckConfiguration configuration = null,
            bool useDefaultCheckServiceOnlineTask = true,
            params Assembly[] healthCheckTaskAssemblies)
        {
            builder.RegisterInstance(configuration ?? new HealthCheckConfiguration()).AsSelf().SingleInstance();
            builder.RegisterType<AutofacHealthCheckTaskProvider>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<HealthCheckExecutor>().AsImplementedInterfaces().InstancePerLifetimeScope();

            if (useDefaultCheckServiceOnlineTask)
            {
                builder.RegisterType<CheckServiceOnlineTask>().AsImplementedInterfaces().InstancePerLifetimeScope();
            }

            builder.RegisterAssemblyTypes(healthCheckTaskAssemblies)
                .Where(t => t.IsAssignableTo<IHealthCheckTask>())
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            return builder;
        }
    }
}