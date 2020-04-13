using System.Threading.Tasks;

namespace RedPanda.Service.HealthCheck
{
    public interface IHealthCheckTask
    {
        Task<HealthCheckTaskResult> DoAsync();
    }
}
