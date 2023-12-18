using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql")]
public class AzCosmosdbSqlContainer
{
    public AzCosmosdbSqlContainer(
        AzCosmosdbSqlContainerCreate create,
        AzCosmosdbSqlContainerThroughput throughput,
        AzCosmosdbSqlContainerUpdate update,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        Throughput = throughput;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbSqlContainerCreate CreateCommands { get; }

    public AzCosmosdbSqlContainerThroughput Throughput { get; }

    public AzCosmosdbSqlContainerUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzCosmosdbSqlContainerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbSqlContainerDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzCosmosdbSqlContainerExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbSqlContainerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Merge(AzCosmosdbSqlContainerMergeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RedistributePartitionThroughput(AzCosmosdbSqlContainerRedistributePartitionThroughputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzCosmosdbSqlContainerRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetrievePartitionThroughput(AzCosmosdbSqlContainerRetrievePartitionThroughputOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbSqlContainerShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCosmosdbSqlContainerUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}