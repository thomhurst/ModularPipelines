using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account")]
public class AzStorageAccountPrivateEndpointConnection
{
    public AzStorageAccountPrivateEndpointConnection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Approve(AzStorageAccountPrivateEndpointConnectionApproveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountPrivateEndpointConnectionApproveOptions(), token);
    }

    public async Task<CommandResult> Delete(AzStorageAccountPrivateEndpointConnectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountPrivateEndpointConnectionDeleteOptions(), token);
    }

    public async Task<CommandResult> Reject(AzStorageAccountPrivateEndpointConnectionRejectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountPrivateEndpointConnectionRejectOptions(), token);
    }

    public async Task<CommandResult> Show(AzStorageAccountPrivateEndpointConnectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAccountPrivateEndpointConnectionShowOptions(), token);
    }
}