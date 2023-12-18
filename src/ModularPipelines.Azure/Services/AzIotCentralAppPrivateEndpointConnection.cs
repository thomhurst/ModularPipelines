using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "app")]
public class AzIotCentralAppPrivateEndpointConnection
{
    public AzIotCentralAppPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzIotCentralAppPrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotCentralAppPrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Delete(AzIotCentralAppPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotCentralAppPrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzIotCentralAppPrivateEndpointConnectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotCentralAppPrivateEndpointConnectionListOptions(), token);
    }

    public async Task<CommandResult> Reject(AzIotCentralAppPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotCentralAppPrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzIotCentralAppPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzIotCentralAppPrivateEndpointConnectionShowOptions(), token);
    }
}