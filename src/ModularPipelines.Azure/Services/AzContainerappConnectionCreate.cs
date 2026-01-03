using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connection")]
public class AzContainerappConnectionCreate
{
    public AzContainerappConnectionCreate(
        AzContainerappConnectionCreateMysqlFlexible mysqlFlexible,
        AzContainerappConnectionCreatePostgres postgres,
        AzContainerappConnectionCreatePostgresFlexible postgresFlexible,
        AzContainerappConnectionCreateSql sql,
        ICommand internalCommand
    )
    {
        MysqlFlexibleCommands = mysqlFlexible;
        Postgres = postgres;
        PostgresFlexibleCommands = postgresFlexible;
        SqlCommands = sql;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzContainerappConnectionCreateMysqlFlexible MysqlFlexibleCommands { get; }

    public AzContainerappConnectionCreatePostgres Postgres { get; }

    public AzContainerappConnectionCreatePostgresFlexible PostgresFlexibleCommands { get; }

    public AzContainerappConnectionCreateSql SqlCommands { get; }

    public async Task<CommandResult> Appconfig(AzContainerappConnectionCreateAppconfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateAppconfigOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ConfluentCloud(AzContainerappConnectionCreateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> CosmosCassandra(AzContainerappConnectionCreateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosCassandraOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosGremlin(AzContainerappConnectionCreateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosGremlinOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosMongo(AzContainerappConnectionCreateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosMongoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosSql(AzContainerappConnectionCreateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosTable(AzContainerappConnectionCreateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Eventhub(AzContainerappConnectionCreateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateEventhubOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Keyvault(AzContainerappConnectionCreateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateKeyvaultOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> MysqlFlexible(AzContainerappConnectionCreateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateMysqlFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PostgresFlexible(AzContainerappConnectionCreatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreatePostgresFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Redis(AzContainerappConnectionCreateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateRedisOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RedisEnterprise(AzContainerappConnectionCreateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateRedisEnterpriseOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Servicebus(AzContainerappConnectionCreateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateServicebusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Signalr(AzContainerappConnectionCreateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateSignalrOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sql(AzContainerappConnectionCreateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageBlob(AzContainerappConnectionCreateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateStorageBlobOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageFile(AzContainerappConnectionCreateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateStorageFileOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageQueue(AzContainerappConnectionCreateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateStorageQueueOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageTable(AzContainerappConnectionCreateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateStorageTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Webpubsub(AzContainerappConnectionCreateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateWebpubsubOptions(), cancellationToken: token);
    }
}