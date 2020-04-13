namespace RedPanda.Service.HealthCheck
{
    public class HealthCheckConfiguration
    {
        public virtual string ServiceName { get; set; } = HealthCheckConsts.DefaultServiceName;

        /// <summary>
        /// 默认判断服务健康的级别为<see cref="HealthyServiceLevel.Warning"/>
        /// </summary>
        public virtual HealthyServiceLevel HealthyServiceLevel { get; set; } = HealthyServiceLevel.Warning;
    }
}
