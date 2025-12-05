using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network", "watcher")]
public class AzNetworkWatcherConnectionMonitor
{
    public AzNetworkWatcherConnectionMonitor(
        AzNetworkWatcherConnectionMonitorEndpoint endpoint,
        AzNetworkWatcherConnectionMonitorOutput output,
        AzNetworkWatcherConnectionMonitorTestConfiguration testConfiguration,
        AzNetworkWatcherConnectionMonitorTestGroup testGroup,
        ICommand internalCommand
    )
    {
        Endpoint = endpoint;
        Output = output;
        TestConfiguration = testConfiguration;
        TestGroup = testGroup;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkWatcherConnectionMonitorEndpoint Endpoint { get; }

    public AzNetworkWatcherConnectionMonitorOutput Output { get; }

    public AzNetworkWatcherConnectionMonitorTestConfiguration TestConfiguration { get; }

    public AzNetworkWatcherConnectionMonitorTestGroup TestGroup { get; }

    public async Task<CommandResult> Create(AzNetworkWatcherConnectionMonitorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkWatcherConnectionMonitorDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkWatcherConnectionMonitorListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Query(AzNetworkWatcherConnectionMonitorQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkWatcherConnectionMonitorShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzNetworkWatcherConnectionMonitorStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzNetworkWatcherConnectionMonitorStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetworkWatcherConnectionMonitorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkWatcherConnectionMonitorWaitOptions(), token);
    }
}