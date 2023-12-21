using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network")]
public class AzNetworkWatcher
{
    public AzNetworkWatcher(
        AzNetworkWatcherConnectionMonitor connectionMonitor,
        AzNetworkWatcherFlowLog flowLog,
        AzNetworkWatcherPacketCapture packetCapture,
        AzNetworkWatcherTroubleshooting troubleshooting,
        ICommand internalCommand
    )
    {
        ConnectionMonitor = connectionMonitor;
        FlowLog = flowLog;
        PacketCapture = packetCapture;
        Troubleshooting = troubleshooting;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkWatcherConnectionMonitor ConnectionMonitor { get; }

    public AzNetworkWatcherFlowLog FlowLog { get; }

    public AzNetworkWatcherPacketCapture PacketCapture { get; }

    public AzNetworkWatcherTroubleshooting Troubleshooting { get; }

    public async Task<CommandResult> Configure(AzNetworkWatcherConfigureOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkWatcherListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkWatcherListOptions(), token);
    }

    public async Task<CommandResult> RunConfigurationDiagnostic(AzNetworkWatcherRunConfigurationDiagnosticOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowNextHop(AzNetworkWatcherShowNextHopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowSecurityGroupView(AzNetworkWatcherShowSecurityGroupViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowTopology(AzNetworkWatcherShowTopologyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkWatcherShowTopologyOptions(), token);
    }

    public async Task<CommandResult> TestConnectivity(AzNetworkWatcherTestConnectivityOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TestIpFlow(AzNetworkWatcherTestIpFlowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}