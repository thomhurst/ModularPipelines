using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "connection")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateAppconfigOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ConfluentCloud(AzSpringConnectionUpdateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> CosmosCassandra(AzSpringConnectionUpdateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosCassandraOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosGremlin(AzSpringConnectionUpdateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosGremlinOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosMongo(AzSpringConnectionUpdateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosMongoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosSql(AzSpringConnectionUpdateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosTable(AzSpringConnectionUpdateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateCosmosTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Eventhub(AzSpringConnectionUpdateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateEventhubOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Keyvault(AzSpringConnectionUpdateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateKeyvaultOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> MysqlFlexible(AzSpringConnectionUpdateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateMysqlFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PostgresFlexible(AzSpringConnectionUpdatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdatePostgresFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Redis(AzSpringConnectionUpdateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateRedisOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RedisEnterprise(AzSpringConnectionUpdateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateRedisEnterpriseOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Servicebus(AzSpringConnectionUpdateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateServicebusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Signalr(AzSpringConnectionUpdateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateSignalrOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sql(AzSpringConnectionUpdateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageBlob(AzSpringConnectionUpdateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateStorageBlobOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageFile(AzSpringConnectionUpdateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateStorageFileOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageQueue(AzSpringConnectionUpdateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateStorageQueueOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageTable(AzSpringConnectionUpdateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateStorageTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Webpubsub(AzSpringConnectionUpdateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionUpdateWebpubsubOptions(), cancellationToken: token);
    }
}