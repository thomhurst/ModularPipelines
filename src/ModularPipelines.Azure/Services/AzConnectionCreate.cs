using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection")]
public class AzConnectionCreate
{
    public AzConnectionCreate(
        AzConnectionCreateMysqlFlexible mysqlFlexible,
        AzConnectionCreatePostgres postgres,
        AzConnectionCreatePostgresFlexible postgresFlexible,
        AzConnectionCreateSql sql,
        ICommand internalCommand
    )
    {
        MysqlFlexibleCommands = mysqlFlexible;
        PostgresCommands = postgres;
        PostgresFlexibleCommands = postgresFlexible;
        SqlCommands = sql;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzConnectionCreateMysqlFlexible MysqlFlexibleCommands { get; }

    public AzConnectionCreatePostgres PostgresCommands { get; }

    public AzConnectionCreatePostgresFlexible PostgresFlexibleCommands { get; }

    public AzConnectionCreateSql SqlCommands { get; }

    public async Task<CommandResult> Appconfig(AzConnectionCreateAppconfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfluentCloud(AzConnectionCreateConfluentCloudOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosCassandra(AzConnectionCreateCosmosCassandraOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosGremlin(AzConnectionCreateCosmosGremlinOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosMongo(AzConnectionCreateCosmosMongoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosSql(AzConnectionCreateCosmosSqlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CosmosTable(AzConnectionCreateCosmosTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Eventhub(AzConnectionCreateEventhubOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Keyvault(AzConnectionCreateKeyvaultOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Mysql(AzConnectionCreateMysqlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MysqlFlexible(AzConnectionCreateMysqlFlexibleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Postgres(AzConnectionCreatePostgresOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PostgresFlexible(AzConnectionCreatePostgresFlexibleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Redis(AzConnectionCreateRedisOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RedisEnterprise(AzConnectionCreateRedisEnterpriseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Servicebus(AzConnectionCreateServicebusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Signalr(AzConnectionCreateSignalrOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Sql(AzConnectionCreateSqlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StorageBlob(AzConnectionCreateStorageBlobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StorageFile(AzConnectionCreateStorageFileOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StorageQueue(AzConnectionCreateStorageQueueOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StorageTable(AzConnectionCreateStorageTableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Webpubsub(AzConnectionCreateWebpubsubOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}