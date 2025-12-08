using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware")]
public class GcloudVmwareNetworkPeerings
{
    public GcloudVmwareNetworkPeerings(
        GcloudVmwareNetworkPeeringsRoutes routes,
        ICommand internalCommand
    )
    {
        Routes = routes;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudVmwareNetworkPeeringsRoutes Routes { get; }

    public async Task<CommandResult> Create(GcloudVmwareNetworkPeeringsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudVmwareNetworkPeeringsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudVmwareNetworkPeeringsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudVmwareNetworkPeeringsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudVmwareNetworkPeeringsListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudVmwareNetworkPeeringsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}