using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "connection")]
public class AzFunctionappConnectionCreate
{
    public AzFunctionappConnectionCreate(
        AzFunctionappConnectionCreateMysqlFlexible mysqlFlexible,
        AzFunctionappConnectionCreatePostgres postgres,
        AzFunctionappConnectionCreatePostgresFlexible postgresFlexible,
        AzFunctionappConnectionCreateSql sql,
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

    public AzFunctionappConnectionCreateMysqlFlexible MysqlFlexibleCommands { get; }

    public AzFunctionappConnectionCreatePostgres Postgres { get; }

    public AzFunctionappConnectionCreatePostgresFlexible PostgresFlexibleCommands { get; }

    public AzFunctionappConnectionCreateSql SqlCommands { get; }

    public async Task<CommandResult> Appconfig(AzFunctionappConnectionCreateAppconfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateAppconfigOptions(), token);
    }

    public async Task<CommandResult> ConfluentCloud(AzFunctionappConnectionCreateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzFunctionappConnectionCreateCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzFunctionappConnectionCreateCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzFunctionappConnectionCreateCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzFunctionappConnectionCreateCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzFunctionappConnectionCreateCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzFunctionappConnectionCreateEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzFunctionappConnectionCreateKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateKeyvaultOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzFunctionappConnectionCreateMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzFunctionappConnectionCreatePostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreatePostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzFunctionappConnectionCreateRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzFunctionappConnectionCreateRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzFunctionappConnectionCreateServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzFunctionappConnectionCreateSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzFunctionappConnectionCreateSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzFunctionappConnectionCreateStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzFunctionappConnectionCreateStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzFunctionappConnectionCreateStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzFunctionappConnectionCreateStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzFunctionappConnectionCreateWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappConnectionCreateWebpubsubOptions(), token);
    }
}