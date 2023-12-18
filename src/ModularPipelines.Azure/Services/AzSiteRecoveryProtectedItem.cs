using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery")]
public class AzSiteRecoveryProtectedItem
{
    public AzSiteRecoveryProtectedItem(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzSiteRecoveryProtectedItemCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSiteRecoveryProtectedItemDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> FailoverCommit(AzSiteRecoveryProtectedItemFailoverCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSiteRecoveryProtectedItemListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PlannedFailover(AzSiteRecoveryProtectedItemPlannedFailoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectedItemPlannedFailoverOptions(), token);
    }

    public async Task<CommandResult> Remove(AzSiteRecoveryProtectedItemRemoveOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectedItemRemoveOptions(), token);
    }

    public async Task<CommandResult> Reprotect(AzSiteRecoveryProtectedItemReprotectOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectedItemReprotectOptions(), token);
    }

    public async Task<CommandResult> Show(AzSiteRecoveryProtectedItemShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectedItemShowOptions(), token);
    }

    public async Task<CommandResult> UnplannedFailover(AzSiteRecoveryProtectedItemUnplannedFailoverOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectedItemUnplannedFailoverOptions(), token);
    }

    public async Task<CommandResult> Update(AzSiteRecoveryProtectedItemUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSiteRecoveryProtectedItemUpdateOptions(), token);
    }
}