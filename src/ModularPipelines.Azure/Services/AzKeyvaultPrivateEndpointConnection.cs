using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault")]
public class AzKeyvaultPrivateEndpointConnection
{
    public AzKeyvaultPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzKeyvaultPrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultPrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Delete(AzKeyvaultPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultPrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzKeyvaultPrivateEndpointConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Reject(AzKeyvaultPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultPrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzKeyvaultPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultPrivateEndpointConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzKeyvaultPrivateEndpointConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzKeyvaultPrivateEndpointConnectionWaitOptions(), token);
    }
}