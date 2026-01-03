using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("workloads", "monitor")]
public class AzWorkloadsMonitorSapLandscapeMonitor
{
    public AzWorkloadsMonitorSapLandscapeMonitor(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzWorkloadsMonitorSapLandscapeMonitorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzWorkloadsMonitorSapLandscapeMonitorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsMonitorSapLandscapeMonitorDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzWorkloadsMonitorSapLandscapeMonitorListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzWorkloadsMonitorSapLandscapeMonitorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsMonitorSapLandscapeMonitorShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzWorkloadsMonitorSapLandscapeMonitorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsMonitorSapLandscapeMonitorUpdateOptions(), cancellationToken: token);
    }
}