using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage")]
public class AzStorageShareRm
{
    public AzStorageShareRm(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzStorageShareRmCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzStorageShareRmDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageShareRmDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Exists(AzStorageShareRmExistsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageShareRmExistsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzStorageShareRmListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Restore(AzStorageShareRmRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzStorageShareRmShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageShareRmShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Snapshot(AzStorageShareRmSnapshotOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageShareRmSnapshotOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stats(AzStorageShareRmStatsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageShareRmStatsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzStorageShareRmUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageShareRmUpdateOptions(), cancellationToken: token);
    }
}