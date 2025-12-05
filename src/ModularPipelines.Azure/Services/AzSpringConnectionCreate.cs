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
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateAppconfigOptions(), token);
    }

    public async Task<CommandResult> ConfluentCloud(AzSpringConnectionCreateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzSpringConnectionCreateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzSpringConnectionCreateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzSpringConnectionCreateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzSpringConnectionCreateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzSpringConnectionCreateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzSpringConnectionCreateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzSpringConnectionCreateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzSpringConnectionCreateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzSpringConnectionCreatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzSpringConnectionCreateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzSpringConnectionCreateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzSpringConnectionCreateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzSpringConnectionCreateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzSpringConnectionCreateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzSpringConnectionCreateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzSpringConnectionCreateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzSpringConnectionCreateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzSpringConnectionCreateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzSpringConnectionCreateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSpringConnectionCreateWebpubsubOptions(), token);
    }
}