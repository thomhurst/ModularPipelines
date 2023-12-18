using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor")]
public class AzMonitorLogAnalytics
{
    public AzMonitorLogAnalytics(
        AzMonitorLogAnalyticsCluster cluster,
        AzMonitorLogAnalyticsQueryPack queryPack,
        AzMonitorLogAnalyticsSolution solution,
        AzMonitorLogAnalyticsWorkspace workspace,
        ICommand internalCommand
    )
    {
        Cluster = cluster;
        QueryPack = queryPack;
        Solution = solution;
        Workspace = workspace;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorLogAnalyticsCluster Cluster { get; }

    public AzMonitorLogAnalyticsQueryPack QueryPack { get; }

    public AzMonitorLogAnalyticsSolution Solution { get; }

    public AzMonitorLogAnalyticsWorkspace Workspace { get; }

    public async Task<CommandResult> Query(AzMonitorLogAnalyticsQueryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}