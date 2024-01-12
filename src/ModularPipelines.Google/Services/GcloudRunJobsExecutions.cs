using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "jobs")]
public class GcloudRunJobsExecutions
{
    public GcloudRunJobsExecutions(
        GcloudRunJobsExecutionsTasks tasks,
        ICommand internalCommand
    )
    {
        Tasks = tasks;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudRunJobsExecutionsTasks Tasks { get; }

    public async Task<CommandResult> Cancel(GcloudRunJobsExecutionsCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudRunJobsExecutionsDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudRunJobsExecutionsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudRunJobsExecutionsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudRunJobsExecutionsListOptions(), token);
    }
}