using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("databricks")]
public class AzDatabricksWorkspace
{
    public AzDatabricksWorkspace(
        AzDatabricksWorkspaceOutboundEndpoint outboundEndpoint,
        AzDatabricksWorkspacePrivateEndpointConnection privateEndpointConnection,
        AzDatabricksWorkspacePrivateLinkResource privateLinkResource,
        AzDatabricksWorkspaceVnetPeering vnetPeering,
        ICommand internalCommand
    )
    {
        OutboundEndpoint = outboundEndpoint;
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        VnetPeering = vnetPeering;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDatabricksWorkspaceOutboundEndpoint OutboundEndpoint { get; }

    public AzDatabricksWorkspacePrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzDatabricksWorkspacePrivateLinkResource PrivateLinkResource { get; }

    public AzDatabricksWorkspaceVnetPeering VnetPeering { get; }

    public async Task<CommandResult> Create(AzDatabricksWorkspaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDatabricksWorkspaceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksWorkspaceDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDatabricksWorkspaceListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksWorkspaceListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDatabricksWorkspaceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksWorkspaceShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatabricksWorkspaceUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksWorkspaceUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDatabricksWorkspaceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatabricksWorkspaceWaitOptions(), token);
    }
}