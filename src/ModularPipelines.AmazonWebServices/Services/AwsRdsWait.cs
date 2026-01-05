using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("rds")]
public class AwsRdsWait
{
    public AwsRdsWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DbClusterAvailable(AwsRdsWaitDbClusterAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbClusterAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbClusterDeleted(AwsRdsWaitDbClusterDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbClusterDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbClusterSnapshotAvailable(AwsRdsWaitDbClusterSnapshotAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbClusterSnapshotAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbClusterSnapshotDeleted(AwsRdsWaitDbClusterSnapshotDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbClusterSnapshotDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbInstanceAvailable(AwsRdsWaitDbInstanceAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbInstanceAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbInstanceDeleted(AwsRdsWaitDbInstanceDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbInstanceDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbSnapshotAvailable(AwsRdsWaitDbSnapshotAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbSnapshotAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbSnapshotCompleted(AwsRdsWaitDbSnapshotCompletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbSnapshotCompletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> DbSnapshotDeleted(AwsRdsWaitDbSnapshotDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbSnapshotDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TenantDatabaseAvailable(AwsRdsWaitTenantDatabaseAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitTenantDatabaseAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TenantDatabaseDeleted(AwsRdsWaitTenantDatabaseDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitTenantDatabaseDeletedOptions(), executionOptions, cancellationToken);
    }
}