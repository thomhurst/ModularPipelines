using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai")]
public class AzBatchaiCluster
{
    public AzBatchaiCluster(
        AzBatchaiClusterFile file,
        AzBatchaiClusterNode node,
        ICommand internalCommand
    )
    {
        File = file;
        Node = node;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchaiClusterFile File { get; }

    public AzBatchaiClusterNode Node { get; }

    public async Task<CommandResult> AutoScale(AzBatchaiClusterAutoScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzBatchaiClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzBatchaiClusterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBatchaiClusterListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resize(AzBatchaiClusterResizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBatchaiClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchaiClusterShowOptions(), token);
    }
}