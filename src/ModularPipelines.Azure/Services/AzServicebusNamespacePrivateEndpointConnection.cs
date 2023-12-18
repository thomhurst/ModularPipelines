using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "namespace")]
public class AzServicebusNamespacePrivateEndpointConnection
{
    public AzServicebusNamespacePrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzServicebusNamespacePrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespacePrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Create(AzServicebusNamespacePrivateEndpointConnectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzServicebusNamespacePrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespacePrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzServicebusNamespacePrivateEndpointConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reject(AzServicebusNamespacePrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespacePrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzServicebusNamespacePrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespacePrivateEndpointConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzServicebusNamespacePrivateEndpointConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespacePrivateEndpointConnectionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzServicebusNamespacePrivateEndpointConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzServicebusNamespacePrivateEndpointConnectionWaitOptions(), token);
    }
}