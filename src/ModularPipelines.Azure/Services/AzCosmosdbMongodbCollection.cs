using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb")]
public class AzCosmosdbMongodbCollection
{
    public AzCosmosdbMongodbCollection(
        AzCosmosdbMongodbCollectionThroughput throughput,
        ICommand internalCommand
    )
    {
        Throughput = throughput;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbMongodbCollectionThroughput Throughput { get; }

    public async Task<CommandResult> Create(AzCosmosdbMongodbCollectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbMongodbCollectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzCosmosdbMongodbCollectionExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbMongodbCollectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Merge(AzCosmosdbMongodbCollectionMergeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RedistributePartitionThroughput(AzCosmosdbMongodbCollectionRedistributePartitionThroughputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzCosmosdbMongodbCollectionRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetrievePartitionThroughput(AzCosmosdbMongodbCollectionRetrievePartitionThroughputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbMongodbCollectionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCosmosdbMongodbCollectionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}