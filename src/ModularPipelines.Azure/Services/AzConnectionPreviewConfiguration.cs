using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection")]
public class AzConnectionPreviewConfiguration
{
    public AzConnectionPreviewConfiguration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Appconfig(AzConnectionPreviewConfigurationAppconfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationAppconfigOptions(), token);
    }

    public async Task<CommandResult> ConfluentCloud(AzConnectionPreviewConfigurationConfluentCloudOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationConfluentCloudOptions(), token);
    }

    public async Task<CommandResult> CosmosCassandra(AzConnectionPreviewConfigurationCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosCassandraOptions(), token);
    }

    public async Task<CommandResult> CosmosGremlin(AzConnectionPreviewConfigurationCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosGremlinOptions(), token);
    }

    public async Task<CommandResult> CosmosMongo(AzConnectionPreviewConfigurationCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosMongoOptions(), token);
    }

    public async Task<CommandResult> CosmosSql(AzConnectionPreviewConfigurationCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosSqlOptions(), token);
    }

    public async Task<CommandResult> CosmosTable(AzConnectionPreviewConfigurationCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosTableOptions(), token);
    }

    public async Task<CommandResult> Eventhub(AzConnectionPreviewConfigurationEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationEventhubOptions(), token);
    }

    public async Task<CommandResult> Keyvault(AzConnectionPreviewConfigurationKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationKeyvaultOptions(), token);
    }

    public async Task<CommandResult> Mysql(AzConnectionPreviewConfigurationMysqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationMysqlOptions(), token);
    }

    public async Task<CommandResult> MysqlFlexible(AzConnectionPreviewConfigurationMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationMysqlFlexibleOptions(), token);
    }

    public async Task<CommandResult> Postgres(AzConnectionPreviewConfigurationPostgresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationPostgresOptions(), token);
    }

    public async Task<CommandResult> PostgresFlexible(AzConnectionPreviewConfigurationPostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationPostgresFlexibleOptions(), token);
    }

    public async Task<CommandResult> Redis(AzConnectionPreviewConfigurationRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationRedisOptions(), token);
    }

    public async Task<CommandResult> RedisEnterprise(AzConnectionPreviewConfigurationRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationRedisEnterpriseOptions(), token);
    }

    public async Task<CommandResult> Servicebus(AzConnectionPreviewConfigurationServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationServicebusOptions(), token);
    }

    public async Task<CommandResult> Signalr(AzConnectionPreviewConfigurationSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationSignalrOptions(), token);
    }

    public async Task<CommandResult> Sql(AzConnectionPreviewConfigurationSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationSqlOptions(), token);
    }

    public async Task<CommandResult> StorageBlob(AzConnectionPreviewConfigurationStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationStorageBlobOptions(), token);
    }

    public async Task<CommandResult> StorageFile(AzConnectionPreviewConfigurationStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationStorageFileOptions(), token);
    }

    public async Task<CommandResult> StorageQueue(AzConnectionPreviewConfigurationStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationStorageQueueOptions(), token);
    }

    public async Task<CommandResult> StorageTable(AzConnectionPreviewConfigurationStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationStorageTableOptions(), token);
    }

    public async Task<CommandResult> Webpubsub(AzConnectionPreviewConfigurationWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationWebpubsubOptions(), token);
    }
}