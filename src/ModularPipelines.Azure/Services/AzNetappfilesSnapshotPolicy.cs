using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "snapshot")]
public class AzNetappfilesSnapshotPolicy
{
    public AzNetappfilesSnapshotPolicy(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetappfilesSnapshotPolicyCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetappfilesSnapshotPolicyDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSnapshotPolicyDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetappfilesSnapshotPolicyListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetappfilesSnapshotPolicyShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSnapshotPolicyShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetappfilesSnapshotPolicyUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSnapshotPolicyUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Volumes(AzNetappfilesSnapshotPolicyVolumesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSnapshotPolicyVolumesOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetappfilesSnapshotPolicyWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesSnapshotPolicyWaitOptions(), cancellationToken: token);
    }
}