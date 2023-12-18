using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai")]
public class AzBatchaiJob
{
    public AzBatchaiJob(
        AzBatchaiJobFile file,
        AzBatchaiJobNode node,
        ICommand internalCommand
    )
    {
        File = file;
        Node = node;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchaiJobFile File { get; }

    public AzBatchaiJobNode Node { get; }

    public async Task<CommandResult> Create(AzBatchaiJobCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzBatchaiJobDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBatchaiJobListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBatchaiJobShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchaiJobShowOptions(), token);
    }

    public async Task<CommandResult> Terminate(AzBatchaiJobTerminateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchaiJobTerminateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzBatchaiJobWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchaiJobWaitOptions(), token);
    }
}