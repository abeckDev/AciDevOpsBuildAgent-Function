using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Management.ContainerRegistry.Fluent.Models;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AciDevOpsBuildAgent_Function
{
    public static class AzureConnectionHelper
    {
        public static IAzure GetAzureInterface(HttpRequest req, RequestBody requestBody)
        {
            //Obtain Function Config from Environment Vars
            string alwaysEmpty = Environment.GetEnvironmentVariable("NeverExists");
            string servicePrincipalClientId = Environment.GetEnvironmentVariable("ServicePrincipal_clientId");
            string servicePrincipalClientSecret = Environment.GetEnvironmentVariable("ServicePrincipal_clientSecret");
            string servicePrincipalTenantId = Environment.GetEnvironmentVariable("ServicePrincipal_tenantId");
            
            //Generating Credential Factory to generate Azure login credentials
            var credentialsFactory = new AzureCredentialsFactory();
            AzureCredentials azureCredentials;
            if (servicePrincipalClientId != null &&
                servicePrincipalClientSecret != null &&
                servicePrincipalTenantId != null)
            {
                //Use servicePrincipal auth for local debugging
                azureCredentials = credentialsFactory.FromServicePrincipal(servicePrincipalClientId, servicePrincipalClientSecret, servicePrincipalTenantId, AzureEnvironment.AzureGlobalCloud);
            }
            else
            {
                //Authenticate to Azure using the Azure SDK and Azure Functions "system assigned managed Identity"
                //Setup of managed Identity needs to be done in the Function settings in the portal or via CLI
                azureCredentials = credentialsFactory.FromSystemAssignedManagedServiceIdentity(MSIResourceType.AppService, AzureEnvironment.AzureGlobalCloud);
            }
            //Authenticate with Azure Credentials to get a working Azure interface (here saved as azure)
            return Azure.Authenticate(azureCredentials).WithSubscription(requestBody.subscriptionId);
        }
    }
}
