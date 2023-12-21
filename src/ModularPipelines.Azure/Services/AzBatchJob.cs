using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch")]
public class AzBatchJob
{
    public AzBatchJob(
        AzBatchJobCreate create,
        AzBatchJobPrepReleaseStatus prepReleaseStatus,
        AzBatchJobTaskCounts taskCounts,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        PrepReleaseStatus = prepReleaseStatus;
        TaskCounts = taskCounts;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzBatchJobCreate CreateCommands { get; }

    public AzBatchJobPrepReleaseStatus PrepReleaseStatus { get; }

    public AzBatchJobTaskCounts TaskCounts { get; }

    public async Task<CommandResult> Create(AzBatchJobCreateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchJobCreateOptions(), token);
    }

    public async Task<CommandResult> Delete(AzBatchJobDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Disable(AzBatchJobDisableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Enable(AzBatchJobEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzBatchJobListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzBatchJobListOptions(), token);
    }

    public async Task<CommandResult> Reset(AzBatchJobResetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Set(AzBatchJobSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzBatchJobShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzBatchJobStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}