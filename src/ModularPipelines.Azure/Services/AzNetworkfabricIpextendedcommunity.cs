using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
public class AzNetworkfabricIpextendedcommunity
{
    public AzNetworkfabricIpextendedcommunity(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricIpextendedcommunityCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricIpextendedcommunityDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricIpextendedcommunityListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricIpextendedcommunityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricIpextendedcommunityUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricIpextendedcommunityWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityWaitOptions(), token);
    }
}