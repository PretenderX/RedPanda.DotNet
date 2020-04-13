using System.Collections.Generic;
using System.Linq;

namespace RedPanda.Service.HealthCheck.Autofac
{
    public class AutofacHealthCheckTaskProvider : IHealthCheckTaskProvider
    {
        private readonly IEnumerable<IHealthCheckTask> healthCheckTasks;

        public AutofacHealthCheckTaskProvider(IEnumerable<IHealthCheckTask> healthCheckTasks)
        {
            this.healthCheckTasks = healthCheckTasks;
        }

        public IList<IHealthCheckTask> GetAll()
        {
            return healthCheckTasks.ToList();
        }
    }
}
