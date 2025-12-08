using System.Diagnostics.CodeAnalysis;
using ModularPipelines.AmazonWebServices.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("neptune-graph")]
public class AwsNeptuneGraphWait
{
    public AwsNeptuneGraphWait(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> GraphAvailable(AwsNeptuneGraphWaitGraphAvailableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GraphDeleted(AwsNeptuneGraphWaitGraphDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GraphSnapshotAvailable(AwsNeptuneGraphWaitGraphSnapshotAvailableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GraphSnapshotDeleted(AwsNeptuneGraphWaitGraphSnapshotDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportTaskCancelled(AwsNeptuneGraphWaitImportTaskCancelledOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImportTaskSuccessful(AwsNeptuneGraphWaitImportTaskSuccessfulOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PrivateGraphEndpointAvailable(AwsNeptuneGraphWaitPrivateGraphEndpointAvailableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PrivateGraphEndpointDeleted(AwsNeptuneGraphWaitPrivateGraphEndpointDeletedOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}