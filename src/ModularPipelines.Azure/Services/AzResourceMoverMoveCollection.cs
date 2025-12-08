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
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionBulkRemoveOptions(), token);
    }

    public async Task<CommandResult> Commit(AzResourceMoverMoveCollectionCommitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionCommitOptions(), token);
    }

    public async Task<CommandResult> Create(AzResourceMoverMoveCollectionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzResourceMoverMoveCollectionDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionDeleteOptions(), token);
    }

    public async Task<CommandResult> Discard(AzResourceMoverMoveCollectionDiscardOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionDiscardOptions(), token);
    }

    public async Task<CommandResult> InitiateMove(AzResourceMoverMoveCollectionInitiateMoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionInitiateMoveOptions(), token);
    }

    public async Task<CommandResult> List(AzResourceMoverMoveCollectionListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionListOptions(), token);
    }

    public async Task<CommandResult> ListRequiredFor(AzResourceMoverMoveCollectionListRequiredForOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListUnresolvedDependency(AzResourceMoverMoveCollectionListUnresolvedDependencyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Prepare(AzResourceMoverMoveCollectionPrepareOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionPrepareOptions(), token);
    }

    public async Task<CommandResult> ResolveDependency(AzResourceMoverMoveCollectionResolveDependencyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionResolveDependencyOptions(), token);
    }

    public async Task<CommandResult> Show(AzResourceMoverMoveCollectionShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzResourceMoverMoveCollectionUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzResourceMoverMoveCollectionWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzResourceMoverMoveCollectionWaitOptions(), token);
    }
}