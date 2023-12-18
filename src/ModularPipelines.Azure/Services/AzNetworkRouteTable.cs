using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkRouteTable
{
    public AzNetworkRouteTable(
        AzNetworkRouteTableRoute route,
        ICommand internalCommand
    )
    {
        Route = route;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkRouteTableRoute Route { get; }

    public async Task<CommandResult> Create(AzNetworkRouteTableCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkRouteTableDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkRouteTableListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkRouteTableShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkRouteTableUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkRouteTableWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkRouteTableWaitOptions(), token);
    }
}