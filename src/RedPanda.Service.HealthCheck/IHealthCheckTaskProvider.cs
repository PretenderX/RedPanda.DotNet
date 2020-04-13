using System.Collections.Generic;

namespace RedPanda.Service.HealthCheck
{
    public interface IHealthCheckTaskProvider
    {
        IList<IHealthCheckTask> GetAll();
    }
}
