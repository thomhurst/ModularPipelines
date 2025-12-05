using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles")]
public class AzNetappfilesSnapshot
{
    public AzNetappfilesSnapshot(
        AzNetappfilesSnapshotPolicy policy,
        ICommand internalCommand
    )
    {
        Policy = policy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetappfilesSnapshotPolicy Policy { get; }

    public async Task<CommandResult> Create(AzNetappfilesSnapshotCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesSnapshotDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSnapshotDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetappfilesSnapshotListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestoreFiles(AzNetappfilesSnapshotRestoreFilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetappfilesSnapshotShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSnapshotShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetappfilesSnapshotUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesSnapshotWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSnapshotWaitOptions(), token);
    }
}