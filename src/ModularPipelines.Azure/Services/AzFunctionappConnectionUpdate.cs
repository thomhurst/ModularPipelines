using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "connection")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateAppconfigOptions(), token);
    }

    public async Task<CommandResult> ConfluentCloud(AzFunctionappConnectionUpdateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzFunctionappConnectionUpdateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzFunctionappConnectionUpdateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzFunctionappConnectionUpdateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzFunctionappConnectionUpdateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzFunctionappConnectionUpdateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzFunctionappConnectionUpdateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzFunctionappConnectionUpdateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzFunctionappConnectionUpdateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzFunctionappConnectionUpdatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzFunctionappConnectionUpdateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzFunctionappConnectionUpdateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzFunctionappConnectionUpdateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzFunctionappConnectionUpdateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzFunctionappConnectionUpdateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzFunctionappConnectionUpdateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzFunctionappConnectionUpdateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzFunctionappConnectionUpdateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzFunctionappConnectionUpdateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzFunctionappConnectionUpdateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionUpdateWebpubsubOptions(), token);
    }
}