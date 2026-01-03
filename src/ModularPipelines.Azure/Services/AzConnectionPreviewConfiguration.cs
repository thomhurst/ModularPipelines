using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("connection")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationAppconfigOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ConfluentCloud(AzConnectionPreviewConfigurationConfluentCloudOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationConfluentCloudOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosCassandra(AzConnectionPreviewConfigurationCosmosCassandraOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosCassandraOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosGremlin(AzConnectionPreviewConfigurationCosmosGremlinOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosGremlinOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosMongo(AzConnectionPreviewConfigurationCosmosMongoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosMongoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosSql(AzConnectionPreviewConfigurationCosmosSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> CosmosTable(AzConnectionPreviewConfigurationCosmosTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationCosmosTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Eventhub(AzConnectionPreviewConfigurationEventhubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationEventhubOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Keyvault(AzConnectionPreviewConfigurationKeyvaultOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationKeyvaultOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Mysql(AzConnectionPreviewConfigurationMysqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationMysqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> MysqlFlexible(AzConnectionPreviewConfigurationMysqlFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationMysqlFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Postgres(AzConnectionPreviewConfigurationPostgresOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationPostgresOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> PostgresFlexible(AzConnectionPreviewConfigurationPostgresFlexibleOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationPostgresFlexibleOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Redis(AzConnectionPreviewConfigurationRedisOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationRedisOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RedisEnterprise(AzConnectionPreviewConfigurationRedisEnterpriseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationRedisEnterpriseOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Servicebus(AzConnectionPreviewConfigurationServicebusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationServicebusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Signalr(AzConnectionPreviewConfigurationSignalrOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationSignalrOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sql(AzConnectionPreviewConfigurationSqlOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationSqlOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageBlob(AzConnectionPreviewConfigurationStorageBlobOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationStorageBlobOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageFile(AzConnectionPreviewConfigurationStorageFileOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationStorageFileOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageQueue(AzConnectionPreviewConfigurationStorageQueueOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationStorageQueueOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> StorageTable(AzConnectionPreviewConfigurationStorageTableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationStorageTableOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Webpubsub(AzConnectionPreviewConfigurationWebpubsubOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectionPreviewConfigurationWebpubsubOptions(), cancellationToken: token);
    }
}