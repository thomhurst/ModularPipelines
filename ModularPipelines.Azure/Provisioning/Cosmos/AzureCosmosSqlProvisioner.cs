using Azure;
using Azure.ResourceManager;
using Azure.ResourceManager.CosmosDB;
using Azure.ResourceManager.CosmosDB.Models;
using ModularPipelines.Azure.Scopes;

namespace ModularPipelines.Azure.Provisioning.Cosmos;

public class AzureCosmosSqlProvisioner : BaseAzureProvisioner
{
    public AzureCosmosSqlProvisioner(ArmClient armClient) : base(armClient)
    {
    }

    public async Task<ArmOperation<CosmosDBSqlDatabaseResource>> Database(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, CosmosDBSqlDatabaseCreateOrUpdateContent properties)
    {
        var account = await GetAccount(azureResourceIdentifier);

        return await account.Value.GetCosmosDBSqlDatabases().CreateOrUpdateAsync(WaitUntil.Completed, databaseName, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlRoleAssignmentResource>> RoleAssignment(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, CosmosDBSqlRoleAssignmentCreateOrUpdateContent properties)
    {
        var account = await GetAccount(azureResourceIdentifier);

        return await account.Value.GetCosmosDBSqlRoleAssignments().CreateOrUpdateAsync(WaitUntil.Completed, databaseName, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlRoleDefinitionResource>> RoleDefinition(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, CosmosDBSqlRoleDefinitionCreateOrUpdateContent properties)
    {
        var account = await GetAccount(azureResourceIdentifier);

        return await account.Value.GetCosmosDBSqlRoleDefinitions().CreateOrUpdateAsync(WaitUntil.Completed, databaseName, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlContainerResource>> Container(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, CosmosDBSqlContainerCreateOrUpdateContent properties)
    {
        var database = await GetDatabase(azureResourceIdentifier, databaseName);

        return await database.Value.GetCosmosDBSqlContainers().CreateOrUpdateAsync(WaitUntil.Completed, containerName, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlTriggerResource>> Trigger(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, string triggerName, CosmosDBSqlTriggerCreateOrUpdateContent properties)
    {
        var container = await GetContainer(azureResourceIdentifier, databaseName, containerName);

        return await container.Value.GetCosmosDBSqlTriggers().CreateOrUpdateAsync(WaitUntil.Completed, triggerName, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlStoredProcedureResource>> StoredProcedure(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, string storedProcedureName, CosmosDBSqlStoredProcedureCreateOrUpdateContent properties)
    {
        var container = await GetContainer(azureResourceIdentifier, databaseName, containerName);

        return await container.Value.GetCosmosDBSqlStoredProcedures().CreateOrUpdateAsync(WaitUntil.Completed, storedProcedureName, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlUserDefinedFunctionResource>> UserDefinedFunction(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, string functionName, CosmosDBSqlUserDefinedFunctionCreateOrUpdateContent properties)
    {
        var container = await GetContainer(azureResourceIdentifier, databaseName, containerName);

        return await container.Value.GetCosmosDBSqlUserDefinedFunctions().CreateOrUpdateAsync(WaitUntil.Completed, functionName, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlDatabaseThroughputSettingResource>> DatabaseThroughputSetting(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, ThroughputSettingsUpdateData properties)
    {
        var database = await GetDatabase(azureResourceIdentifier, databaseName);

        return await database.Value.GetCosmosDBSqlDatabaseThroughputSetting().CreateOrUpdateAsync(WaitUntil.Completed, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlContainerThroughputSettingResource>> ContainerThroughputSetting(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, ThroughputSettingsUpdateData properties)
    {
        var container = await GetContainer(azureResourceIdentifier, databaseName, containerName);

        return await container.Value.GetCosmosDBSqlContainerThroughputSetting().CreateOrUpdateAsync(WaitUntil.Completed, properties);
    }

    public async Task<ArmOperation<CosmosDBSqlClientEncryptionKeyResource>> EncryptionKey(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string encryptionKeyName, CosmosDBSqlClientEncryptionKeyCreateOrUpdateContent properties)
    {
        var database = await GetDatabase(azureResourceIdentifier, databaseName);

        return await database.Value.GetCosmosDBSqlClientEncryptionKeys().CreateOrUpdateAsync(WaitUntil.Completed, encryptionKeyName, properties);
    }

    public async Task<ArmOperation<CosmosDBTableResource>> Table(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, CosmosDBTableCreateOrUpdateContent properties)
    {
        var account = await GetAccount(azureResourceIdentifier);

        return await account.Value.GetCosmosDBTables().CreateOrUpdateAsync(WaitUntil.Completed, databaseName, properties);
    }

    public async Task<Response<CosmosDBSqlContainerResource>> GetContainer(AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName)
    {
        var database = await GetDatabase(azureResourceIdentifier, databaseName);

        return await database.Value.GetCosmosDBSqlContainerAsync(containerName);
    }

    public async Task<Response<CosmosDBSqlDatabaseResource>> GetDatabase(AzureResourceIdentifier azureResourceIdentifier, string databaseName)
    {
        var account = await GetAccount(azureResourceIdentifier);

        return await account.Value.GetCosmosDBSqlDatabaseAsync(databaseName);
    }

    public async Task<Response<CosmosDBAccountResource>> GetAccount(AzureResourceIdentifier azureResourceIdentifier)
    {
        return await GetResourceGroup(azureResourceIdentifier).GetCosmosDBAccountAsync(azureResourceIdentifier.ResourceName);
    }
}