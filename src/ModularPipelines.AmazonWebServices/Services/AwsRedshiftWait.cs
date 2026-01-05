using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public async Task<CommandResult> ClusterAvailable(AwsRedshiftWaitClusterAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftWaitClusterAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ClusterDeleted(AwsRedshiftWaitClusterDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftWaitClusterDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ClusterRestored(AwsRedshiftWaitClusterRestoredOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftWaitClusterRestoredOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> SnapshotAvailable(AwsRedshiftWaitSnapshotAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRedshiftWaitSnapshotAvailableOptions(), executionOptions, cancellationToken);
    }
}