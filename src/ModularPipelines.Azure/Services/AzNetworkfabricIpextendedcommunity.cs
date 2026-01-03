using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricIpextendedcommunityDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkfabricIpextendedcommunityListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricIpextendedcommunityShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricIpextendedcommunityUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricIpextendedcommunityWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricIpextendedcommunityWaitOptions(), cancellationToken: token);
    }
}