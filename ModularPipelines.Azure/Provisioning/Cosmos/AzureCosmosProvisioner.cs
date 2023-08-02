using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.CosmosDB;
using Azure.ResourceManager.CosmosDB.Models;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Cosmos;

public class AzureCosmosProvisioner : BaseAzureProvisioner
{
    public AzureCosmosSqlProvisioner Sql { get; }

    public AzureCosmosProvisioner(ArmClient armClient, AzureCosmosSqlProvisioner sql) : base(armClient)
    {
        Sql = sql;
    }

    public async Task<ArmOperation<CosmosDBAccountResource>> Account(
        AzureResourceIdentifier azureResourceIdentifier, CosmosDBAccountCreateOrUpdateContent properties)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetCosmosDBAccounts()
            .CreateOrUpdateAsync(WaitUntil.Completed, azureResourceIdentifier.ResourceName, properties);
    }
}