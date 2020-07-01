using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AciDevOpsBuildAgent_Function
{
    public class RequestBody
    {
        public string subscriptionId { get; set; }

        public string ResourceGroupName { get; set; }

        public string AzureRegion { get; set; }

        public string ContainerInstanceName { get; set; }

        public RegistrySettings RegistrySettings { get; set; }

        public ContainerSettings ContainerSettings { get; set; }

        public Dictionary<string,string> BuildMetaData { get; set; }
    }

    public class RegistrySettings
    {
        public string LoginServer { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }

    public class ContainerSettings
    {
        public string DockerImageName { get; set; }

        public int ExternalTcpPort { get; set; }

        public double CpuCoreCount { get; set; } = 1.0;

        public int MemorySizeInGb { get; set; } = 1;

        public string DnsPrefix { get; set; }
    }
}
