using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms")]
public class AwsDmsWait
{
    public AwsDmsWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> EndpointDeleted(AwsDmsWaitEndpointDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitEndpointDeletedOptions(), token);
    }

    public async Task<CommandResult> ReplicationInstanceAvailable(AwsDmsWaitReplicationInstanceAvailableOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationInstanceAvailableOptions(), token);
    }

    public async Task<CommandResult> ReplicationInstanceDeleted(AwsDmsWaitReplicationInstanceDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationInstanceDeletedOptions(), token);
    }

    public async Task<CommandResult> ReplicationTaskDeleted(AwsDmsWaitReplicationTaskDeletedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationTaskDeletedOptions(), token);
    }

    public async Task<CommandResult> ReplicationTaskReady(AwsDmsWaitReplicationTaskReadyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationTaskReadyOptions(), token);
    }

    public async Task<CommandResult> ReplicationTaskRunning(AwsDmsWaitReplicationTaskRunningOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationTaskRunningOptions(), token);
    }

    public async Task<CommandResult> ReplicationTaskStopped(AwsDmsWaitReplicationTaskStoppedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitReplicationTaskStoppedOptions(), token);
    }

    public async Task<CommandResult> TestConnectionSucceeds(AwsDmsWaitTestConnectionSucceedsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AwsDmsWaitTestConnectionSucceedsOptions(), token);
    }
}