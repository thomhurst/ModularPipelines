using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("dms")]
public class AwsDmsWait
{
    public AwsDmsWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> EndpointDeleted(AwsDmsWaitEndpointDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitEndpointDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplicationInstanceAvailable(AwsDmsWaitReplicationInstanceAvailableOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationInstanceAvailableOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplicationInstanceDeleted(AwsDmsWaitReplicationInstanceDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationInstanceDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplicationTaskDeleted(AwsDmsWaitReplicationTaskDeletedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationTaskDeletedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplicationTaskReady(AwsDmsWaitReplicationTaskReadyOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationTaskReadyOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplicationTaskRunning(AwsDmsWaitReplicationTaskRunningOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationTaskRunningOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> ReplicationTaskStopped(AwsDmsWaitReplicationTaskStoppedOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationTaskStoppedOptions(), executionOptions, cancellationToken);
    }

    public async Task<CommandResult> TestConnectionSucceeds(AwsDmsWaitTestConnectionSucceedsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitTestConnectionSucceedsOptions(), executionOptions, cancellationToken);
    }
}