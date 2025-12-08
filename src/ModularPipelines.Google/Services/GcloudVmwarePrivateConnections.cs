using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware")]
public class GcloudVmwarePrivateConnections
{
    public GcloudVmwarePrivateConnections(
        GcloudVmwarePrivateConnectionsRoutes routes,
        ICommand internalCommand
    )
    {
        Routes = routes;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudVmwarePrivateConnectionsRoutes Routes { get; }

    public async Task<CommandResult> Create(GcloudVmwarePrivateConnectionsCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudVmwarePrivateConnectionsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudVmwarePrivateConnectionsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudVmwarePrivateConnectionsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudVmwarePrivateConnectionsListOptions(), token);
    }

    public async Task<CommandResult> Update(GcloudVmwarePrivateConnectionsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}