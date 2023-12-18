using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine")]
public class AzConnectedmachinePrivateEndpointConnection
{
    public AzConnectedmachinePrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Delete(AzConnectedmachinePrivateEndpointConnectionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzConnectedmachinePrivateEndpointConnectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzConnectedmachinePrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateEndpointConnectionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzConnectedmachinePrivateEndpointConnectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateEndpointConnectionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzConnectedmachinePrivateEndpointConnectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedmachinePrivateEndpointConnectionWaitOptions(), token);
    }
}