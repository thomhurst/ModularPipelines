using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connection")]
public class AzContainerappConnectionUpdate
{
    public AzContainerappConnectionUpdate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Appconfig(AzContainerappConnectionUpdateAppconfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateAppconfigOptions(), token);
    }

    public async Task<CommandResult> ConfluentCloud(AzContainerappConnectionUpdateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzContainerappConnectionUpdateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzContainerappConnectionUpdateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzContainerappConnectionUpdateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzContainerappConnectionUpdateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzContainerappConnectionUpdateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzContainerappConnectionUpdateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzContainerappConnectionUpdateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzContainerappConnectionUpdateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzContainerappConnectionUpdatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzContainerappConnectionUpdateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzContainerappConnectionUpdateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzContainerappConnectionUpdateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzContainerappConnectionUpdateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzContainerappConnectionUpdateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzContainerappConnectionUpdateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzContainerappConnectionUpdateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzContainerappConnectionUpdateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzContainerappConnectionUpdateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzContainerappConnectionUpdateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateWebpubsubOptions(), token);
    }
}