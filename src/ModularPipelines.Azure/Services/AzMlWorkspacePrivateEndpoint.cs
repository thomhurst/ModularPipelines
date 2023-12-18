using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "workspace")]
public class AzMlWorkspacePrivateEndpoint
{
    public AzMlWorkspacePrivateEndpoint(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzMlWorkspacePrivateEndpointAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspacePrivateEndpointAddOptions(), token);
    }

    public async Task<CommandResult> Delete(AzMlWorkspacePrivateEndpointDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspacePrivateEndpointDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMlWorkspacePrivateEndpointListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMlWorkspacePrivateEndpointListOptions(), token);
    }
}