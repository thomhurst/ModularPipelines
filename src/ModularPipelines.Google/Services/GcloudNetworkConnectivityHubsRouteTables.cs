using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "hubs")]
public class GcloudNetworkConnectivityHubsRouteTables
{
    public GcloudNetworkConnectivityHubsRouteTables(
        GcloudNetworkConnectivityHubsRouteTablesRoutes routes,
        ICommand internalCommand
    )
    {
        Routes = routes;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudNetworkConnectivityHubsRouteTablesRoutes Routes { get; }

    public async Task<CommandResult> Describe(GcloudNetworkConnectivityHubsRouteTablesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudNetworkConnectivityHubsRouteTablesListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}