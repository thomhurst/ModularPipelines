using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vpn-connection")]
public class AzNetworkVpnConnectionPacketCapture
{
    public AzNetworkVpnConnectionPacketCapture(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Start(AzNetworkVpnConnectionPacketCaptureStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnConnectionPacketCaptureStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzNetworkVpnConnectionPacketCaptureStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkVpnConnectionPacketCaptureWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVpnConnectionPacketCaptureWaitOptions(), token);
    }
}