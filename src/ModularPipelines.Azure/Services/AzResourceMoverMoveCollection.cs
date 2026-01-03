using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("resource-mover")]
public class AzResourceMoverMoveCollection
{
    public AzResourceMoverMoveCollection(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> BulkRemove(AzResourceMoverMoveCollectionBulkRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionBulkRemoveOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Commit(AzResourceMoverMoveCollectionCommitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionCommitOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzResourceMoverMoveCollectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzResourceMoverMoveCollectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Discard(AzResourceMoverMoveCollectionDiscardOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionDiscardOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> InitiateMove(AzResourceMoverMoveCollectionInitiateMoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionInitiateMoveOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzResourceMoverMoveCollectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListRequiredFor(AzResourceMoverMoveCollectionListRequiredForOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListUnresolvedDependency(AzResourceMoverMoveCollectionListUnresolvedDependencyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Prepare(AzResourceMoverMoveCollectionPrepareOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionPrepareOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ResolveDependency(AzResourceMoverMoveCollectionResolveDependencyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionResolveDependencyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzResourceMoverMoveCollectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzResourceMoverMoveCollectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzResourceMoverMoveCollectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionWaitOptions(), cancellationToken: token);
    }
}