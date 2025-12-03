using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vnet-gateway")]
public class AzNetworkVnetGatewayPacketCapture
{
    public AzNetworkVnetGatewayPacketCapture(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Start(AzNetworkVnetGatewayPacketCaptureStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayPacketCaptureStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzNetworkVnetGatewayPacketCaptureStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayPacketCaptureStopOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkVnetGatewayPacketCaptureWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkVnetGatewayPacketCaptureWaitOptions(), token);
    }
}