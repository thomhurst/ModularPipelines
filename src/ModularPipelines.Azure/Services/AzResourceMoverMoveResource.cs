using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource-mover")]
public class AzResourceMoverMoveResource
{
    public AzResourceMoverMoveResource(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzResourceMoverMoveResourceAddOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveResourceAddOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzResourceMoverMoveResourceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveResourceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzResourceMoverMoveResourceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzResourceMoverMoveResourceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveResourceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzResourceMoverMoveResourceWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveResourceWaitOptions(), cancellationToken: token);
    }
}