using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "route-table")]
public class AzNetworkRouteTableRoute
{
    public AzNetworkRouteTableRoute(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkRouteTableRouteCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkRouteTableRouteDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableRouteDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkRouteTableRouteListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkRouteTableRouteShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableRouteShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkRouteTableRouteUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableRouteUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkRouteTableRouteWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableRouteWaitOptions(), token);
    }
}