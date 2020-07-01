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
            log.LogInformation("C# HTTP trigger function processed a request.");

            RequestBody requestBody = null;
            try
            {
                //Obtain session parameters from the request
                requestBody = JsonConvert.DeserializeObject<RequestBody>(new StreamReader(req.Body).ReadToEnd());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            IAzure azure = AzureConnectionHelper.GetAzureInterface(req, requestBody);

            //Setup Azure Container Instance
            try
            {
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

                return new OkObjectResult(containerInstance.Name);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }       
        }
    }
}
