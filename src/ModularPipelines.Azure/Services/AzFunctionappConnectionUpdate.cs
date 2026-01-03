using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "connection")]
public class AzFunctionappConnectionUpdate
{
    public AzFunctionappConnectionUpdate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Appconfig(AzFunctionappConnectionUpdateAppconfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateAppconfigOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ConfluentCloud(AzFunctionappConnectionUpdateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> CosmosCassandra(AzFunctionappConnectionUpdateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosCassandraOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosGremlin(AzFunctionappConnectionUpdateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosGremlinOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosMongo(AzFunctionappConnectionUpdateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosMongoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosSql(AzFunctionappConnectionUpdateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosTable(AzFunctionappConnectionUpdateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Eventhub(AzFunctionappConnectionUpdateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateEventhubOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Keyvault(AzFunctionappConnectionUpdateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateKeyvaultOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> MysqlFlexible(AzFunctionappConnectionUpdateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateMysqlFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PostgresFlexible(AzFunctionappConnectionUpdatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdatePostgresFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Redis(AzFunctionappConnectionUpdateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateRedisOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RedisEnterprise(AzFunctionappConnectionUpdateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateRedisEnterpriseOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Servicebus(AzFunctionappConnectionUpdateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateServicebusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Signalr(AzFunctionappConnectionUpdateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateSignalrOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sql(AzFunctionappConnectionUpdateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageBlob(AzFunctionappConnectionUpdateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateStorageBlobOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageFile(AzFunctionappConnectionUpdateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateStorageFileOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageQueue(AzFunctionappConnectionUpdateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateStorageQueueOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageTable(AzFunctionappConnectionUpdateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateStorageTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Webpubsub(AzFunctionappConnectionUpdateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateWebpubsubOptions(), cancellationToken: token);
    }
}