using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp")]
public class GcloudNetappVolumes
{
    public GcloudNetappVolumes(
        GcloudNetappVolumesReplications replications,
        GcloudNetappVolumesSnapshots snapshots,
        ICommand internalCommand
    )
    {
        Replications = replications;
        Snapshots = snapshots;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudNetappVolumesReplications Replications { get; }

    public GcloudNetappVolumesSnapshots Snapshots { get; }

    public async Task<CommandResult> Create(GcloudNetappVolumesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudNetappVolumesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudNetappVolumesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudNetappVolumesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudNetappVolumesListOptions(), token);
    }

    public async Task<CommandResult> Revert(GcloudNetappVolumesRevertOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudNetappVolumesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}