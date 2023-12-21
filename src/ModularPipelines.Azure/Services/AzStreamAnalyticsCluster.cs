using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics")]
public class AzStreamAnalyticsCluster
{
    public AzStreamAnalyticsCluster(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzStreamAnalyticsClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStreamAnalyticsClusterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsClusterDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzStreamAnalyticsClusterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsClusterListOptions(), token);
    }

    public async Task<CommandResult> ListStreamingJob(AzStreamAnalyticsClusterListStreamingJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStreamAnalyticsClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsClusterShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzStreamAnalyticsClusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsClusterUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzStreamAnalyticsClusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStreamAnalyticsClusterWaitOptions(), token);
    }
}