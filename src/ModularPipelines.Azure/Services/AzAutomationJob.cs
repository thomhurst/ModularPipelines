using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("automation")]
public class AzAutomationJob
{
    public AzAutomationJob(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzAutomationJobListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Resume(AzAutomationJobResumeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationJobResumeOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzAutomationJobShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationJobShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzAutomationJobStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationJobStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Suspend(AzAutomationJobSuspendOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzAutomationJobSuspendOptions(), cancellationToken: token);
    }
}