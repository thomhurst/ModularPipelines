using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric")]
public class AzNetworkfabricNeighborgroup
{
    public AzNetworkfabricNeighborgroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricNeighborgroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricNeighborgroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricNeighborgroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricNeighborgroupListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricNeighborgroupListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricNeighborgroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricNeighborgroupShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricNeighborgroupUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricNeighborgroupUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricNeighborgroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricNeighborgroupWaitOptions(), token);
    }
}