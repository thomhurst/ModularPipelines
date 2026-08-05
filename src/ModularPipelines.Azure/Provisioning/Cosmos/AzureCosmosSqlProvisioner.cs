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

    public async Task<ArmOperation<CosmosDBSqlDatabaseResource>> DatabaseAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, CosmosDBSqlDatabaseCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var account = await GetAccountAsync(azureResourceIdentifier, cancellationToken);

        return await account.Value.GetCosmosDBSqlDatabases().CreateOrUpdateAsync(WaitUntil.Completed, databaseName, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlRoleAssignmentResource>> RoleAssignmentAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, CosmosDBSqlRoleAssignmentCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var account = await GetAccountAsync(azureResourceIdentifier, cancellationToken);

        return await account.Value.GetCosmosDBSqlRoleAssignments().CreateOrUpdateAsync(WaitUntil.Completed, databaseName, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlRoleDefinitionResource>> RoleDefinitionAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, CosmosDBSqlRoleDefinitionCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var account = await GetAccountAsync(azureResourceIdentifier, cancellationToken);

        return await account.Value.GetCosmosDBSqlRoleDefinitions().CreateOrUpdateAsync(WaitUntil.Completed, databaseName, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlContainerResource>> ContainerAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, CosmosDBSqlContainerCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var database = await GetDatabaseAsync(azureResourceIdentifier, databaseName, cancellationToken);

        return await database.Value.GetCosmosDBSqlContainers().CreateOrUpdateAsync(WaitUntil.Completed, containerName, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlTriggerResource>> TriggerAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, string triggerName, CosmosDBSqlTriggerCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(triggerName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var container = await GetContainerAsync(azureResourceIdentifier, databaseName, containerName, cancellationToken);

        return await container.Value.GetCosmosDBSqlTriggers().CreateOrUpdateAsync(WaitUntil.Completed, triggerName, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlStoredProcedureResource>> StoredProcedureAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, string storedProcedureName, CosmosDBSqlStoredProcedureCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(storedProcedureName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var container = await GetContainerAsync(azureResourceIdentifier, databaseName, containerName, cancellationToken);

        return await container.Value.GetCosmosDBSqlStoredProcedures().CreateOrUpdateAsync(WaitUntil.Completed, storedProcedureName, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlUserDefinedFunctionResource>> UserDefinedFunctionAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, string functionName, CosmosDBSqlUserDefinedFunctionCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(functionName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var container = await GetContainerAsync(azureResourceIdentifier, databaseName, containerName, cancellationToken);

        return await container.Value.GetCosmosDBSqlUserDefinedFunctions().CreateOrUpdateAsync(WaitUntil.Completed, functionName, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlDatabaseThroughputSettingResource>> DatabaseThroughputSettingAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, ThroughputSettingsUpdateData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var database = await GetDatabaseAsync(azureResourceIdentifier, databaseName, cancellationToken);

        return await database.Value.GetCosmosDBSqlDatabaseThroughputSetting().CreateOrUpdateAsync(WaitUntil.Completed, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlContainerThroughputSettingResource>> ContainerThroughputSettingAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, ThroughputSettingsUpdateData properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var container = await GetContainerAsync(azureResourceIdentifier, databaseName, containerName, cancellationToken);

        return await container.Value.GetCosmosDBSqlContainerThroughputSetting().CreateOrUpdateAsync(WaitUntil.Completed, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBSqlClientEncryptionKeyResource>> EncryptionKeyAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, string encryptionKeyName, CosmosDBSqlClientEncryptionKeyCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(encryptionKeyName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var database = await GetDatabaseAsync(azureResourceIdentifier, databaseName, cancellationToken);

        return await database.Value.GetCosmosDBSqlClientEncryptionKeys().CreateOrUpdateAsync(WaitUntil.Completed, encryptionKeyName, properties, cancellationToken);
    }

    public async Task<ArmOperation<CosmosDBTableResource>> TableAsync(
        AzureResourceIdentifier azureResourceIdentifier, string databaseName, CosmosDBTableCreateOrUpdateContent properties, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var account = await GetAccountAsync(azureResourceIdentifier, cancellationToken);

        return await account.Value.GetCosmosDBTables().CreateOrUpdateAsync(WaitUntil.Completed, databaseName, properties, cancellationToken);
    }

    public async Task<Response<CosmosDBSqlContainerResource>> GetContainerAsync(AzureResourceIdentifier azureResourceIdentifier, string databaseName, string containerName, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(containerName);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var database = await GetDatabaseAsync(azureResourceIdentifier, databaseName, cancellationToken);

        return await database.Value.GetCosmosDBSqlContainerAsync(containerName, cancellationToken);
    }

    public async Task<Response<CosmosDBSqlDatabaseResource>> GetDatabaseAsync(AzureResourceIdentifier azureResourceIdentifier, string databaseName, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(databaseName);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        var account = await GetAccountAsync(azureResourceIdentifier, cancellationToken);

        return await account.Value.GetCosmosDBSqlDatabaseAsync(databaseName, cancellationToken);
    }

    public async Task<Response<CosmosDBAccountResource>> GetAccountAsync(AzureResourceIdentifier azureResourceIdentifier, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(azureResourceIdentifier);
        ArgumentException.ThrowIfNullOrWhiteSpace(azureResourceIdentifier.ResourceName);

        return await GetResourceGroup(azureResourceIdentifier).GetCosmosDBAccountAsync(azureResourceIdentifier.ResourceName, cancellationToken);
    }
}
