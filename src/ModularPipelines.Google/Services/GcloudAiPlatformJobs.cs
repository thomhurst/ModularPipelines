using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform")]
public class GcloudAiPlatformJobs
{
    public GcloudAiPlatformJobs(
        GcloudAiPlatformJobsSubmit submit,
        ICommand internalCommand
    )
    {
        Submit = submit;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudAiPlatformJobsSubmit Submit { get; }

    public async Task<CommandResult> Cancel(GcloudAiPlatformJobsCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudAiPlatformJobsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudAiPlatformJobsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudAiPlatformJobsListOptions(), token);
    }

    public async Task<CommandResult> StreamLogs(GcloudAiPlatformJobsStreamLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(GcloudAiPlatformJobsUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}