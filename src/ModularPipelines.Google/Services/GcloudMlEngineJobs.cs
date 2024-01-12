using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml-engine")]
public class GcloudMlEngineJobs
{
    public GcloudMlEngineJobs(
        GcloudMlEngineJobsSubmit submit,
        ICommand internalCommand
    )
    {
        Submit = submit;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudMlEngineJobsSubmit Submit { get; }

    public async Task<CommandResult> Cancel(GcloudMlEngineJobsCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudMlEngineJobsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudMlEngineJobsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudMlEngineJobsListOptions(), token);
    }

    public async Task<CommandResult> StreamLogs(GcloudMlEngineJobsStreamLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudMlEngineJobsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}