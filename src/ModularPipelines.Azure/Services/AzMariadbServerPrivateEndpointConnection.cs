using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mariadb", "server")]
public class AzMariadbServerPrivateEndpointConnection
{
    public AzMariadbServerPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzMariadbServerPrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerPrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Delete(AzMariadbServerPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerPrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> Reject(AzMariadbServerPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerPrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzMariadbServerPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMariadbServerPrivateEndpointConnectionShowOptions(), token);
    }
}