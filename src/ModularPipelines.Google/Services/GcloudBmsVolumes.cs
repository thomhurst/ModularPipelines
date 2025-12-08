using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("bms")]
public class GcloudBmsVolumes
{
    public GcloudBmsVolumes(
        GcloudBmsVolumesLuns luns,
        GcloudBmsVolumesSnapshots snapshots,
        ICommand internalCommand
    )
    {
        Luns = luns;
        Snapshots = snapshots;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudBmsVolumesLuns Luns { get; }

    public GcloudBmsVolumesSnapshots Snapshots { get; }

    public async Task<CommandResult> Describe(GcloudBmsVolumesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudBmsVolumesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudBmsVolumesListOptions(), token);
    }

    public async Task<CommandResult> Rename(GcloudBmsVolumesRenameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(GcloudBmsVolumesRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Snapshot(GcloudBmsVolumesSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudBmsVolumesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}