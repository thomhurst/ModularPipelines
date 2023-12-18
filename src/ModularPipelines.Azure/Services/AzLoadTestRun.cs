using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load")]
public class AzLoadTestRun
{
    public AzLoadTestRun(
        AzLoadTestRunAppComponent appComponent,
        AzLoadTestRunMetrics metrics,
        AzLoadTestRunServerMetric serverMetric,
        ICommand internalCommand
    )
    {
        AppComponent = appComponent;
        Metrics = metrics;
        ServerMetric = serverMetric;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzLoadTestRunAppComponent AppComponent { get; }

    public AzLoadTestRunMetrics Metrics { get; }

    public AzLoadTestRunServerMetric ServerMetric { get; }

    public async Task<CommandResult> Create(AzLoadTestRunCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzLoadTestRunDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> DownloadFiles(AzLoadTestRunDownloadFilesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzLoadTestRunListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzLoadTestRunShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzLoadTestRunStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzLoadTestRunUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}