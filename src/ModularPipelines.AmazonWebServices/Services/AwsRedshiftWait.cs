using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("redshift")]
public class AwsRedshiftWait
{
    public AwsRedshiftWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> ClusterAvailable(AwsRedshiftWaitClusterAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftWaitClusterAvailableOptions(), token);
    }

    public async Task<CommandResult> ClusterDeleted(AwsRedshiftWaitClusterDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftWaitClusterDeletedOptions(), token);
    }

    public async Task<CommandResult> ClusterRestored(AwsRedshiftWaitClusterRestoredOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftWaitClusterRestoredOptions(), token);
    }

    public async Task<CommandResult> SnapshotAvailable(AwsRedshiftWaitSnapshotAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftWaitSnapshotAvailableOptions(), token);
    }
}