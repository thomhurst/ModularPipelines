using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("dla")]
public class AzDlaJob
{
    public AzDlaJob(
        AzDlaJobPipeline pipeline,
        AzDlaJobRecurrence recurrence,
        ICommand internalCommand
    )
    {
        Pipeline = pipeline;
        Recurrence = recurrence;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDlaJobPipeline Pipeline { get; }

    public AzDlaJobRecurrence Recurrence { get; }

    public async Task<CommandResult> Cancel(AzDlaJobCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDlaJobListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDlaJobListOptions(), token);
    }

    public async Task<CommandResult> Show(AzDlaJobShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Submit(AzDlaJobSubmitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzDlaJobWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}