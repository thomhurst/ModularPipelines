using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("postgres", "server")]
public class AzPostgresServerPrivateEndpointConnection
{
    public AzPostgresServerPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzPostgresServerPrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerPrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Delete(AzPostgresServerPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerPrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> Reject(AzPostgresServerPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerPrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzPostgresServerPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzPostgresServerPrivateEndpointConnectionShowOptions(), token);
    }
}

