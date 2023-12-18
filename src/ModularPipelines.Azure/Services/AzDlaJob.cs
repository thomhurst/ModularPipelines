using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla")]
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

    public async Task<CommandResult> List(AzDlaJobListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
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

