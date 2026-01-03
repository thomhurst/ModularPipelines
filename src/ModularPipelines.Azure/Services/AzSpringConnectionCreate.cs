using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "connection")]
public class AzSpringConnectionCreate
{
    public AzSpringConnectionCreate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Appconfig(AzSpringConnectionCreateAppconfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateAppconfigOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ConfluentCloud(AzSpringConnectionCreateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> CosmosCassandra(AzSpringConnectionCreateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosCassandraOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosGremlin(AzSpringConnectionCreateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosGremlinOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosMongo(AzSpringConnectionCreateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosMongoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosSql(AzSpringConnectionCreateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosTable(AzSpringConnectionCreateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Eventhub(AzSpringConnectionCreateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateEventhubOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Keyvault(AzSpringConnectionCreateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateKeyvaultOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> MysqlFlexible(AzSpringConnectionCreateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateMysqlFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PostgresFlexible(AzSpringConnectionCreatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreatePostgresFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Redis(AzSpringConnectionCreateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateRedisOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RedisEnterprise(AzSpringConnectionCreateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateRedisEnterpriseOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Servicebus(AzSpringConnectionCreateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateServicebusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Signalr(AzSpringConnectionCreateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateSignalrOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sql(AzSpringConnectionCreateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageBlob(AzSpringConnectionCreateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateStorageBlobOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageFile(AzSpringConnectionCreateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateStorageFileOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageQueue(AzSpringConnectionCreateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateStorageQueueOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageTable(AzSpringConnectionCreateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateStorageTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Webpubsub(AzSpringConnectionCreateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateWebpubsubOptions(), cancellationToken: token);
    }
}