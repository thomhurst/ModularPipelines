using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics")]
public class AzMonitorLogAnalyticsCluster
{
    public AzMonitorLogAnalyticsCluster(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMonitorLogAnalyticsClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMonitorLogAnalyticsClusterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsClusterDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMonitorLogAnalyticsClusterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsClusterListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMonitorLogAnalyticsClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsClusterShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMonitorLogAnalyticsClusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsClusterUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzMonitorLogAnalyticsClusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsClusterWaitOptions(), cancellationToken: token);
    }
}