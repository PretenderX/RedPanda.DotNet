namespace RedPanda.Service.Governance
{
    public class ServiceDescription
    {
        public string ServiceSpace { get; set; }

        public string ServiceName { get; set; }

        /// <summary>
        /// http or https
        /// </summary>
        public string ServiceSchema { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string HealthCheckRoute { get; set; }
    }
}
