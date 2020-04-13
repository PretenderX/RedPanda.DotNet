namespace RedPanda.Service.HealthCheck
{
    public enum HealthyServiceLevel
    {
        /// <summary>
        /// 全局健康级别为Excelent时，服务是健康的
        /// </summary>
        Excelent,

        /// <summary>
        /// 全局健康级别为Good时，服务是健康的
        /// </summary>
        Good,

        /// <summary>
        /// 全局健康级别为Warning时，服务是健康的
        /// </summary>
        Warning
    }
}
