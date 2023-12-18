using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "connection")]
public class AzSpringCloudConnectionCreate
{
    public AzSpringCloudConnectionCreate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Appconfig(AzSpringCloudConnectionCreateAppconfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfluentCloud(AzSpringCloudConnectionCreateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzSpringCloudConnectionCreateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzSpringCloudConnectionCreateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzSpringCloudConnectionCreateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzSpringCloudConnectionCreateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzSpringCloudConnectionCreateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzSpringCloudConnectionCreateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzSpringCloudConnectionCreateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> Mysql(AzSpringCloudConnectionCreateMysqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateMysqlOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzSpringCloudConnectionCreateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> Postgres(AzSpringCloudConnectionCreatePostgresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreatePostgresOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzSpringCloudConnectionCreatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzSpringCloudConnectionCreateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzSpringCloudConnectionCreateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzSpringCloudConnectionCreateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzSpringCloudConnectionCreateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzSpringCloudConnectionCreateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzSpringCloudConnectionCreateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzSpringCloudConnectionCreateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzSpringCloudConnectionCreateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzSpringCloudConnectionCreateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzSpringCloudConnectionCreateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringCloudConnectionCreateWebpubsubOptions(), token);
    }
}

