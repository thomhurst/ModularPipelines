using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudTasks
{
    public GcloudTasks(
        GcloudTasksCmekConfig cmekConfig,
        GcloudTasksLocations locations,
        GcloudTasksQueues queues,
        ICommand internalCommand
    )
    {
        CmekConfig = cmekConfig;
        Locations = locations;
        Queues = queues;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudTasksCmekConfig CmekConfig { get; }

    public GcloudTasksLocations Locations { get; }

    public GcloudTasksQueues Queues { get; }

    public async Task<CommandResult> CreateAppEngineTask(GcloudTasksCreateAppEngineTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CreateHttpTask(GcloudTasksCreateHttpTaskOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(GcloudTasksDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudTasksDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudTasksListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Run(GcloudTasksRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}