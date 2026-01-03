using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "server")]
public class AzMysqlServerPrivateEndpointConnection
{
    public AzMysqlServerPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzMysqlServerPrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerPrivateEndpointConnectionApproveOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMysqlServerPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerPrivateEndpointConnectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Reject(AzMysqlServerPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerPrivateEndpointConnectionRejectOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMysqlServerPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMysqlServerPrivateEndpointConnectionShowOptions(), cancellationToken: token);
    }
}