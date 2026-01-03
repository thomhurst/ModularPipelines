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
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateAppconfigOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ConfluentCloud(AzContainerappConnectionUpdateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> CosmosCassandra(AzContainerappConnectionUpdateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosCassandraOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosGremlin(AzContainerappConnectionUpdateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosGremlinOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosMongo(AzContainerappConnectionUpdateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosMongoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosSql(AzContainerappConnectionUpdateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosTable(AzContainerappConnectionUpdateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateCosmosTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Eventhub(AzContainerappConnectionUpdateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateEventhubOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Keyvault(AzContainerappConnectionUpdateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateKeyvaultOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> MysqlFlexible(AzContainerappConnectionUpdateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateMysqlFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PostgresFlexible(AzContainerappConnectionUpdatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdatePostgresFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Redis(AzContainerappConnectionUpdateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateRedisOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RedisEnterprise(AzContainerappConnectionUpdateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateRedisEnterpriseOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Servicebus(AzContainerappConnectionUpdateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateServicebusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Signalr(AzContainerappConnectionUpdateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateSignalrOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sql(AzContainerappConnectionUpdateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageBlob(AzContainerappConnectionUpdateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateStorageBlobOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageFile(AzContainerappConnectionUpdateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateStorageFileOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageQueue(AzContainerappConnectionUpdateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateStorageQueueOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageTable(AzContainerappConnectionUpdateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateStorageTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Webpubsub(AzContainerappConnectionUpdateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionUpdateWebpubsubOptions(), cancellationToken: token);
    }
}