using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "peering")]
public class AzNetworkExpressRoutePeeringConnection
{
    public AzNetworkExpressRoutePeeringConnection(
        AzNetworkExpressRoutePeeringConnectionIpv6Config ipv6Config,
        ICommand internalCommand
    )
    {
        Ipv6Config = ipv6Config;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkExpressRoutePeeringConnectionIpv6Config Ipv6Config { get; }

    public async Task<CommandResult> Create(AzNetworkExpressRoutePeeringConnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkExpressRoutePeeringConnectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkExpressRoutePeeringConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkExpressRoutePeeringConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePeeringConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkExpressRoutePeeringConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePeeringConnectionWaitOptions(), token);
    }
}

