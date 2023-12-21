using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route")]
public class AzNetworkExpressRoutePort
{
    public AzNetworkExpressRoutePort(
        AzNetworkExpressRoutePortIdentity identity,
        AzNetworkExpressRoutePortLink link,
        AzNetworkExpressRoutePortLocation location,
        ICommand internalCommand
    )
    {
        Identity = identity;
        Link = link;
        Location = location;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkExpressRoutePortIdentity Identity { get; }

    public AzNetworkExpressRoutePortLink Link { get; }

    public AzNetworkExpressRoutePortLocation Location { get; }

    public async Task<CommandResult> Create(AzNetworkExpressRoutePortCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkExpressRoutePortDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePortDeleteOptions(), token);
    }

    public async Task<CommandResult> GenerateLoa(AzNetworkExpressRoutePortGenerateLoaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkExpressRoutePortListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePortListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkExpressRoutePortShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePortShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkExpressRoutePortUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePortUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkExpressRoutePortWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePortWaitOptions(), token);
    }
}