using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs")]
public class AzEventhubsCluster
{
    public AzEventhubsCluster(
        AzEventhubsClusterNamespace @namespace,
        ICommand internalCommand
    )
    {
        Namespace = @namespace;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzEventhubsClusterNamespace Namespace { get; }

    public async Task<CommandResult> AvailableRegion(AzEventhubsClusterAvailableRegionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsClusterAvailableRegionOptions(), token);
    }

    public async Task<CommandResult> Create(AzEventhubsClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEventhubsClusterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsClusterDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEventhubsClusterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsClusterListOptions(), token);
    }

    public async Task<CommandResult> Show(AzEventhubsClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsClusterShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEventhubsClusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsClusterUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzEventhubsClusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEventhubsClusterWaitOptions(), token);
    }
}