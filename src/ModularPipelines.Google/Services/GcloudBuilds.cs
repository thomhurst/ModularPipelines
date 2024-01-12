using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudBuilds
{
    public GcloudBuilds(
        GcloudBuildsConnections connections,
        GcloudBuildsRepositories repositories,
        GcloudBuildsTriggers triggers,
        GcloudBuildsWorkerPools workerPools,
        ICommand internalCommand
    )
    {
        Connections = connections;
        Repositories = repositories;
        Triggers = triggers;
        WorkerPools = workerPools;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudBuildsConnections Connections { get; }

    public GcloudBuildsRepositories Repositories { get; }

    public GcloudBuildsTriggers Triggers { get; }

    public GcloudBuildsWorkerPools WorkerPools { get; }

    public async Task<CommandResult> Cancel(GcloudBuildsCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(GcloudBuildsDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(GcloudBuildsListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudBuildsListOptions(), token);
    }

    public async Task<CommandResult> Log(GcloudBuildsLogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Submit(GcloudBuildsSubmitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}