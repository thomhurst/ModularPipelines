using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliCommand("rds")]
public class AwsRdsWait
{
    public AwsRdsWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> DbClusterAvailable(AwsRdsWaitDbClusterAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbClusterAvailableOptions(), token);
    }

    public async Task<CommandResult> DbClusterDeleted(AwsRdsWaitDbClusterDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbClusterDeletedOptions(), token);
    }

    public async Task<CommandResult> DbClusterSnapshotAvailable(AwsRdsWaitDbClusterSnapshotAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbClusterSnapshotAvailableOptions(), token);
    }

    public async Task<CommandResult> DbClusterSnapshotDeleted(AwsRdsWaitDbClusterSnapshotDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbClusterSnapshotDeletedOptions(), token);
    }

    public async Task<CommandResult> DbInstanceAvailable(AwsRdsWaitDbInstanceAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbInstanceAvailableOptions(), token);
    }

    public async Task<CommandResult> DbInstanceDeleted(AwsRdsWaitDbInstanceDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbInstanceDeletedOptions(), token);
    }

    public async Task<CommandResult> DbSnapshotAvailable(AwsRdsWaitDbSnapshotAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbSnapshotAvailableOptions(), token);
    }

    public async Task<CommandResult> DbSnapshotCompleted(AwsRdsWaitDbSnapshotCompletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbSnapshotCompletedOptions(), token);
    }

    public async Task<CommandResult> DbSnapshotDeleted(AwsRdsWaitDbSnapshotDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitDbSnapshotDeletedOptions(), token);
    }

    public async Task<CommandResult> TenantDatabaseAvailable(AwsRdsWaitTenantDatabaseAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitTenantDatabaseAvailableOptions(), token);
    }

    public async Task<CommandResult> TenantDatabaseDeleted(AwsRdsWaitTenantDatabaseDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsRdsWaitTenantDatabaseDeletedOptions(), token);
    }
}