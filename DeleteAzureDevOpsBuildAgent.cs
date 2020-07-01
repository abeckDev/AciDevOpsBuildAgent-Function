using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AciDevOpsBuildAgent_Function;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.ContainerInstance.Fluent;
using System.Linq;

namespace AbeckDev.AzureFunctions
{
    public static class DeleteAzureDevOpsBuildAgent
    {
        [FunctionName("DeleteAzureDevOpsBuildAgent")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            //Read POST Body and bind content to Model for further processing
            RequestBody requestBody = RequestHelper.GetRequestBody(req);

            //Get Azure Interface from Azure helper class
            IAzure azure = AzureConnectionHelper.GetAzureInterface(req, requestBody);

            var AciInstances = await azure.ContainerGroups.ListByResourceGroupAsync(requestBody.ResourceGroupName);
            if (!isAciContainerPresent(AciInstances,requestBody.ContainerInstanceName))
            {
                return new BadRequestObjectResult("Container instance not found!");
            }
            await azure.ContainerGroups.DeleteByResourceGroupAsync(requestBody.ResourceGroupName, requestBody.ContainerInstanceName);
            return new OkObjectResult("OK");
        }

        public static bool isAciContainerPresent(IPagedCollection<IContainerGroup> containerGroups, string ContainerName)
        {
            if (containerGroups.FirstOrDefault(c => c.Name == ContainerName) == null)
            {
                return false;
            }
            return true;
        }
    }
}
