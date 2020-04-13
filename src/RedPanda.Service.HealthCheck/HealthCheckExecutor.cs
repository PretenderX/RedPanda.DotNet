using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedPanda.Service.HealthCheck
{
    public class HealthCheckExecutor : IHealthCheckExecutor
    {
        private readonly HealthCheckConfiguration healthCheckConfiguration;
        private readonly IHealthCheckTaskProvider healthCheckTaskProvider;

        public HealthCheckExecutor(
            HealthCheckConfiguration healthCheckConfiguration,
            IHealthCheckTaskProvider healthCheckTaskProvider)
        {
            this.healthCheckConfiguration = healthCheckConfiguration;
            this.healthCheckTaskProvider = healthCheckTaskProvider;
        }

        public async Task<HealthCheckResult> ExecuteAsync()
        {
            var healthCheckTasks = new List<Task<HealthCheckTaskResult>>();

            foreach (var task in healthCheckTaskProvider.GetAll())
            {
                healthCheckTasks.Add(task.DoAsync());
            }

            var healthCheckTaskResults = await Task.WhenAll(healthCheckTasks);
            var healthLevel = IdentifyHealthLevel(healthCheckTaskResults);
            var isHealthy = healthLevel < HealthLevel.Bad && (int)healthLevel <= (int)healthCheckConfiguration.HealthyServiceLevel;
            
            return new HealthCheckResult
            {
                ServiceName = healthCheckConfiguration.ServiceName,
                IsHealthy = isHealthy,
                HealthLevel = healthLevel.ToString(),
                Details = healthCheckTaskResults,
            };
        }

        private HealthLevel IdentifyHealthLevel(HealthCheckTaskResult[] healthCheckTaskResults)
        {
            if (healthCheckTaskResults.All(r => r.IsSuccessful))
            {
                return HealthLevel.Excelent;
            }

            if (healthCheckTaskResults.All(r => !r.IsOptional && !r.IsSuccessful))
            {
                return HealthLevel.Critical;
            }
            else if (healthCheckTaskResults.Any(r => !r.IsOptional && !r.IsSuccessful))
            {
                return HealthLevel.Bad;
            }
            else if (healthCheckTaskResults.All(r => r.IsOptional && !r.IsSuccessful))
            {
                return HealthLevel.Warning;
            }
         
            return HealthLevel.Good;
        }
    }
}
