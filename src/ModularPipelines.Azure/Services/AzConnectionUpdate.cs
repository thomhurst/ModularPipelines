using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection")]
public class AzConnectionUpdate
{
    public AzConnectionUpdate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Appconfig(AzConnectionUpdateAppconfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfluentCloud(AzConnectionUpdateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzConnectionUpdateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzConnectionUpdateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzConnectionUpdateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzConnectionUpdateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzConnectionUpdateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzConnectionUpdateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzConnectionUpdateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> Mysql(AzConnectionUpdateMysqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateMysqlOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzConnectionUpdateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> Postgres(AzConnectionUpdatePostgresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdatePostgresOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzConnectionUpdatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzConnectionUpdateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzConnectionUpdateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzConnectionUpdateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzConnectionUpdateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzConnectionUpdateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzConnectionUpdateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzConnectionUpdateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzConnectionUpdateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzConnectionUpdateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzConnectionUpdateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionUpdateWebpubsubOptions(), token);
    }
}