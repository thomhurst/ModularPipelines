using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "connection")]
public class AzWebappConnectionUpdate
{
    public AzWebappConnectionUpdate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Appconfig(AzWebappConnectionUpdateAppconfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfluentCloud(AzWebappConnectionUpdateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzWebappConnectionUpdateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzWebappConnectionUpdateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzWebappConnectionUpdateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzWebappConnectionUpdateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzWebappConnectionUpdateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzWebappConnectionUpdateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzWebappConnectionUpdateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> Mysql(AzWebappConnectionUpdateMysqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateMysqlOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzWebappConnectionUpdateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> Postgres(AzWebappConnectionUpdatePostgresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdatePostgresOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzWebappConnectionUpdatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzWebappConnectionUpdateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzWebappConnectionUpdateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzWebappConnectionUpdateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzWebappConnectionUpdateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzWebappConnectionUpdateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzWebappConnectionUpdateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzWebappConnectionUpdateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzWebappConnectionUpdateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzWebappConnectionUpdateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzWebappConnectionUpdateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionUpdateWebpubsubOptions(), token);
    }
}