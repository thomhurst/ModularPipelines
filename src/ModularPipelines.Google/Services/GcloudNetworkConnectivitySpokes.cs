using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("network-connectivity")]
public class GcloudNetworkConnectivitySpokes
{
    public GcloudNetworkConnectivitySpokes(
        GcloudNetworkConnectivitySpokesLinkedInterconnectAttachments linkedInterconnectAttachments,
        GcloudNetworkConnectivitySpokesLinkedRouterAppliances linkedRouterAppliances,
        GcloudNetworkConnectivitySpokesLinkedVpcNetwork linkedVpcNetwork,
        GcloudNetworkConnectivitySpokesLinkedVpnTunnels linkedVpnTunnels,
        ICommand internalCommand
    )
    {
        LinkedInterconnectAttachments = linkedInterconnectAttachments;
        LinkedRouterAppliances = linkedRouterAppliances;
        LinkedVpcNetwork = linkedVpcNetwork;
        LinkedVpnTunnels = linkedVpnTunnels;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudNetworkConnectivitySpokesLinkedInterconnectAttachments LinkedInterconnectAttachments { get; }

    public GcloudNetworkConnectivitySpokesLinkedRouterAppliances LinkedRouterAppliances { get; }

    public GcloudNetworkConnectivitySpokesLinkedVpcNetwork LinkedVpcNetwork { get; }

    public GcloudNetworkConnectivitySpokesLinkedVpnTunnels LinkedVpnTunnels { get; }

    public async Task<CommandResult> Delete(GcloudNetworkConnectivitySpokesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudNetworkConnectivitySpokesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudNetworkConnectivitySpokesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudNetworkConnectivitySpokesListOptions(), token);
    }
}