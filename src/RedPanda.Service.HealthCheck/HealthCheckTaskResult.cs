namespace RedPanda.Service.HealthCheck
{
    public class HealthCheckTaskResult
    {
        public string TaskName { get; set; } = HealthCheckConsts.CheckServiceOnlineTask;

        public bool IsOptional { get; set; } = true;

        public bool IsSuccessful { get; set; } = true;

        public string Message { get; set; } = HealthCheckConsts.DefaultStatusMessage;
    }
}
