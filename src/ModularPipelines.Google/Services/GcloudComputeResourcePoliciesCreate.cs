using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "resource-policies")]
public class GcloudComputeResourcePoliciesCreate
{
    public GcloudComputeResourcePoliciesCreate(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DiskConsistencyGroup(GcloudComputeResourcePoliciesCreateDiskConsistencyGroupOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GroupPlacement(GcloudComputeResourcePoliciesCreateGroupPlacementOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InstanceSchedule(GcloudComputeResourcePoliciesCreateInstanceScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SnapshotSchedule(GcloudComputeResourcePoliciesCreateSnapshotScheduleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}