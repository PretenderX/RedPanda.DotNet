namespace RedPanda.Service.HealthCheck
{
    public class HealthCheckResult
    {
        public string ServiceName { get; set; } = HealthCheckConsts.DefaultServiceName;

        public bool IsHealthy { get; set; } = true;

        public string HealthLevel { get; set; } = HealthCheck.HealthLevel.Excelent.ToString();

        public HealthCheckTaskResult[] Details { get; set; } = new HealthCheckTaskResult[] { new HealthCheckTaskResult() };
    }
}
