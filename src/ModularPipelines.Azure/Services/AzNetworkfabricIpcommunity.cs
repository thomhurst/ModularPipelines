using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
public class AzNetworkfabricIpcommunity
{
    public AzNetworkfabricIpcommunity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricIpcommunityCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricIpcommunityDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpcommunityDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricIpcommunityListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpcommunityListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricIpcommunityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpcommunityShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricIpcommunityUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpcommunityUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricIpcommunityWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpcommunityWaitOptions(), token);
    }
}