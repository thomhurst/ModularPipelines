using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "express-route", "peering", "connection")]
public class AzNetworkExpressRoutePeeringConnectionIpv6Config
{
    public AzNetworkExpressRoutePeeringConnectionIpv6Config(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Remove(AzNetworkExpressRoutePeeringConnectionIpv6ConfigRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzNetworkExpressRoutePeeringConnectionIpv6ConfigSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkExpressRoutePeeringConnectionIpv6ConfigWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkExpressRoutePeeringConnectionIpv6ConfigWaitOptions(), token);
    }
}