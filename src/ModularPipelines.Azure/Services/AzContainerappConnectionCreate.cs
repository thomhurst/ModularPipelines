using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connection")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateAppconfigOptions(), token);
    }

    public async Task<CommandResult> ConfluentCloud(AzContainerappConnectionCreateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzContainerappConnectionCreateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzContainerappConnectionCreateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzContainerappConnectionCreateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzContainerappConnectionCreateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzContainerappConnectionCreateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzContainerappConnectionCreateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzContainerappConnectionCreateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzContainerappConnectionCreateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzContainerappConnectionCreatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzContainerappConnectionCreateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzContainerappConnectionCreateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzContainerappConnectionCreateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzContainerappConnectionCreateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzContainerappConnectionCreateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzContainerappConnectionCreateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzContainerappConnectionCreateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzContainerappConnectionCreateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzContainerappConnectionCreateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzContainerappConnectionCreateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzContainerappConnectionCreateWebpubsubOptions(), token);
    }
}