namespace RedPanda.Service.Governance
{
    public static class  ServiceGovernanceConsts
    {
        public const string ServiceSpace = "ServiceSpace";
        public const string ServiceName = "ServiceName";
        public const string ServiceAliases = "ServiceAliases";
        public const string ServiceSchema = "ServiceSchema";
        public const string ServiceHost = "ServiceHost";
        public const string ServicePort = "ServicePort";
        public const string ServiceVirtualDirectory = "ServiceVirtualDirectory";
        public const string ServiceHealthRoute = "ServiceHealthCheckRoute";

        public const string ServiceCheckInterval = "ServiceCheckInterval";
        public const string ServiceCheckTimeout = "ServiceCheckTimeout";
        public const string DeregisterCriticalServiceAfter = "DeregisterCriticalServiceAfter";

        public const string ConsulAddress = "ConsulAddress";
        public const string ConsulToken = "ConsulToken";

        public const char JsonPropertyNameSplitChar = ':';

        public const string DefaultConsulAddress = "http://localhost:8500";
        public const string DefaultFullServiceNameFormat = "{0}.{1}";
        public const string DefaultServiceSchema = "http";
        public const int DefaultServicePort = 80;
        public const int DefaultServiceCheckTimeout = 30;
        public const int DefaultServiceCheckInterval = 10;
    }
}
