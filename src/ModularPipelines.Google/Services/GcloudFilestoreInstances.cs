using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliCommand("filestore")]
public class GcloudFilestoreInstances
{
    public GcloudFilestoreInstances(
        GcloudFilestoreInstancesSnapshots snapshots,
        ICommand internalCommand
    )
    {
        Snapshots = snapshots;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudFilestoreInstancesSnapshots Snapshots { get; }

    public async Task<CommandResult> Create(GcloudFilestoreInstancesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudFilestoreInstancesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudFilestoreInstancesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudFilestoreInstancesListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudFilestoreInstancesListOptions(), token);
    }

    public async Task<CommandResult> Restore(GcloudFilestoreInstancesRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudFilestoreInstancesUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}