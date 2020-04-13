using System.Threading.Tasks;

namespace RedPanda.Service.HealthCheck
{
    public interface IHealthCheckExecutor
    {
        Task<HealthCheckResult> ExecuteAsync();
    }
}
