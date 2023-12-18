using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "connection")]
public class AzWebappConnectionCreate
{
    public AzWebappConnectionCreate(
        AzWebappConnectionCreateMysqlFlexible mysqlFlexible,
        AzWebappConnectionCreatePostgres postgres,
        AzWebappConnectionCreatePostgresFlexible postgresFlexible,
        AzWebappConnectionCreateSql sql,
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

    public AzWebappConnectionCreateMysqlFlexible MysqlFlexibleCommands { get; }

    public AzWebappConnectionCreatePostgres Postgres { get; }

    public AzWebappConnectionCreatePostgresFlexible PostgresFlexibleCommands { get; }

    public AzWebappConnectionCreateSql SqlCommands { get; }

    public async Task<CommandResult> Appconfig(AzWebappConnectionCreateAppconfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateAppconfigOptions(), token);
    }

    public async Task<CommandResult> ConfluentCloud(AzWebappConnectionCreateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzWebappConnectionCreateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzWebappConnectionCreateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzWebappConnectionCreateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzWebappConnectionCreateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzWebappConnectionCreateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzWebappConnectionCreateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzWebappConnectionCreateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzWebappConnectionCreateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzWebappConnectionCreatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzWebappConnectionCreateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzWebappConnectionCreateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzWebappConnectionCreateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzWebappConnectionCreateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzWebappConnectionCreateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzWebappConnectionCreateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzWebappConnectionCreateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzWebappConnectionCreateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzWebappConnectionCreateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzWebappConnectionCreateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappConnectionCreateWebpubsubOptions(), token);
    }
}