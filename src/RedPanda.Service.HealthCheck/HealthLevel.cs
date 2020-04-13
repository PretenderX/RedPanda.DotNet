namespace RedPanda.Service.HealthCheck
{
    public enum HealthLevel
    {
        /// <summary>
        /// 极好
        /// </summary>
        Excelent,

        /// <summary>
        /// 好
        /// </summary>
        Good,

        /// <summary>
        /// 警告
        /// </summary>
        Warning,

        /// <summary>
        /// 不可用
        /// </summary>
        Bad,

        /// <summary>
        /// 严重不可用
        /// </summary>
        Critical,
    }
}
