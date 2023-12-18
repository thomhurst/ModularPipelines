using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "connection")]
public class AzSpringConnectionUpdate
{
    public AzSpringConnectionUpdate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Appconfig(AzSpringConnectionUpdateAppconfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateAppconfigOptions(), token);
    }

    public async Task<CommandResult> ConfluentCloud(AzSpringConnectionUpdateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzSpringConnectionUpdateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzSpringConnectionUpdateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzSpringConnectionUpdateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzSpringConnectionUpdateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzSpringConnectionUpdateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzSpringConnectionUpdateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzSpringConnectionUpdateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzSpringConnectionUpdateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzSpringConnectionUpdatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzSpringConnectionUpdateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzSpringConnectionUpdateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzSpringConnectionUpdateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzSpringConnectionUpdateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzSpringConnectionUpdateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzSpringConnectionUpdateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzSpringConnectionUpdateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzSpringConnectionUpdateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzSpringConnectionUpdateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzSpringConnectionUpdateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateWebpubsubOptions(), token);
    }
}