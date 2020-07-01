# AciDevOpsBuildAgent-Function
A set of Azure Functions to setup Azure DevOps Build Agents just in time using Azure Container Instances.


## Getting Started
ToDo: Add a Intoduction to project and explaint setup process. 

### Create Azure DevOps Build Agent via Azure Function

To create an Azure Container Instance with an Azure Function call the ```SetupAzureDevOpsBuildAgent``` Function and include a Json body with the following content:

```json
{
	"SubscriptionId": "<SubscriptionId>",
	"ResourceGroupName": "<ResourceGroupName>",
	"AzureRegion": "Westeurope", 
	"ContainerInstanceName": "<ContainerInstanceName>",
	"RegistrySettings":{
		"LoginServer":"<PrivateRegistryLoginServer>",
		"Username": "<Username>",
		"Password": "<Password>"
	},
	"ContainerSettings":{
		"DockerImageName": "<DockerImage>",
		"ExternalTcpPort": "<PortToExposeIfNeededAsInt>",
		"CpuCoreCount": "<CpuCoresAsDouble>",
		"MemorySizeInGb": "<MemorySizeInGbInInt>",
		"DnsPrefix": "<dnsPrefix>"
		
	},
	"BuildMetaData": {
		"PlanUrl": "TagValue1", 
		"ProjectId": "TagValue2",
		"RandomTag": "Value"
	}
}
```
```BuildMetaDataSection``` will be used for KeyValue tagging the ACI resource in Azure for easier management. I recommend to pass Azure DevOps build information. 

#### Sample request

```bash
curl --request POST \
  --url http://localhost:7071/api/SetupAzureDevOpsBuildAgent \
  --header 'content-type: application/json' \
  --data '{
	"SubscriptionId": "<SubscriptionId>",
	"ResourceGroupName": "<ResourceGroupName>",
	"AzureRegion": "Westeurope", 
	"ContainerInstanceName": "<ContainerInstanceName>",
	"RegistrySettings":{
		"LoginServer":"<PrivateRegistryLoginServer>",
		"Username": "<Username>",
		"Password": "<Password>"
	},
	"ContainerSettings":{
		"DockerImageName": "<DockerImage>",
		"ExternalTcpPort": "<PortToExposeIfNeededAsInt>",
		"CpuCoreCount": "<CpuCoresAsDouble>",
		"MemorySizeInGb": "<MemorySizeInGbInInt>",
		"DnsPrefix": "<dnsPrefix>"
		
	},
	"BuildMetaData": {
		"PlanUrl": "TagValue1", 
		"ProjectId": "TagValue2",
		"RandomTag": "Value"
	}
}'
```

### Included Components 

* Microsoft Azure - As main Cloud Environment
  * Azure Functions - Main Entrypoint for REST Clients and Connector to Azure IoT Central
  
**Documentation in Detail will (hopefully) follow soon!**


### Prerequisites

ToDo: Add what things you need to install the software and how to install them

### Installing


## Running the tests


## Deployment

ToDo: Add deployment details

## Built With

* [Azure Functions](https://azure.microsoft.com/en-us/services/functions/) - Serverless Framework which can be triggered by Azure DevOps
* [.NET Core](https://dotnet.microsoft.com/learn/dotnet/what-is-dotnet) - The Framework used to build the Device Client software

## Authors

* **Alexander Beck** - *Initial work* - [abeckdev](https://github.com/abeckDev)

See also the list of [contributors](https://github.com/abeckDev/AciDevOpsBuildAgent-Function/graphs/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments



  
  
