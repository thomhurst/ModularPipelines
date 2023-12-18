using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "private-link-scope")]
public class AzMonitorPrivateLinkScopePrivateEndpointConnection
{
    public AzMonitorPrivateLinkScopePrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzMonitorPrivateLinkScopePrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopePrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Delete(AzMonitorPrivateLinkScopePrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopePrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorPrivateLinkScopePrivateEndpointConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reject(AzMonitorPrivateLinkScopePrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopePrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorPrivateLinkScopePrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopePrivateEndpointConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMonitorPrivateLinkScopePrivateEndpointConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopePrivateEndpointConnectionWaitOptions(), token);
    }
}