using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using AciDevOpsBuildAgent_Function;
using Microsoft.Azure.Management.ContainerRegistry.Fluent.Models;
using System.Linq;
using System.Collections.Generic;

namespace AbeckDev.AzureFunctions
{
    public static class SetupAzureDevOpsBuildAgent
    {
        [FunctionName("SetupAzureDevOpsBuildAgent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //Say hi to the audience
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Read POST Body and bind content to Model for further processing
            RequestBody requestBody = RequestHelper.GetRequestBody(req);

            //Get Azure Interface from Azure helper class
            IAzure azure = AzureConnectionHelper.GetAzureInterface(req, requestBody);

            //Setup Azure Container Instance
            try
            {
                //Define Container Instance based on request body input
                var containerInstance = await azure.ContainerGroups.Define(requestBody.ContainerInstanceName)
                .WithRegion(requestBody.AzureRegion)
                .WithExistingResourceGroup(requestBody.ResourceGroupName)
                .WithLinux()
                .WithPrivateImageRegistry(requestBody.RegistrySettings.LoginServer, requestBody.RegistrySettings.Username, requestBody.RegistrySettings.Password)
                .WithoutVolume()
                .DefineContainerInstance(requestBody.ContainerInstanceName)
                    .WithImage(requestBody.ContainerSettings.DockerImageName)
                    .WithExternalTcpPort(requestBody.ContainerSettings.ExternalTcpPort)
                    .WithCpuCoreCount(requestBody.ContainerSettings.CpuCoreCount)
                    .WithMemorySizeInGB(requestBody.ContainerSettings.MemorySizeInGb)
                    .Attach()
                .WithDnsPrefix(requestBody.ContainerSettings.DnsPrefix)
                .WithTags(requestBody.BuildMetaData)
                .CreateAsync();
                //Return ACI name after creation
                return new OkObjectResult(containerInstance.Name);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }       
        }
    }
}
