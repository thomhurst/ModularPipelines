using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor")]
public class AzMonitorPrivateLinkScope
{
    public AzMonitorPrivateLinkScope(
        AzMonitorPrivateLinkScopePrivateEndpointConnection privateEndpointConnection,
        AzMonitorPrivateLinkScopePrivateLinkResource privateLinkResource,
        AzMonitorPrivateLinkScopeScopedResource scopedResource,
        ICommand internalCommand
    )
    {
        PrivateEndpointConnection = privateEndpointConnection;
        PrivateLinkResource = privateLinkResource;
        ScopedResource = scopedResource;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorPrivateLinkScopePrivateEndpointConnection PrivateEndpointConnection { get; }

    public AzMonitorPrivateLinkScopePrivateLinkResource PrivateLinkResource { get; }

    public AzMonitorPrivateLinkScopeScopedResource ScopedResource { get; }

    public async Task<CommandResult> Create(AzMonitorPrivateLinkScopeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorPrivateLinkScopeDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopeDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorPrivateLinkScopeListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopeListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorPrivateLinkScopeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopeShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorPrivateLinkScopeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopeUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMonitorPrivateLinkScopeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorPrivateLinkScopeWaitOptions(), token);
    }
}