using System.Threading.Tasks;

namespace RedPanda.Service.HealthCheck
{
    public class CheckServiceOnlineTask : IHealthCheckTask
    {
        public Task<HealthCheckTaskResult> DoAsync()
        {
            return Task.FromResult(new HealthCheckTaskResult { IsOptional = false });
        }
    }
}
