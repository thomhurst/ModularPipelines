using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads")]
public class AzWorkloadsMonitor
{
    public AzWorkloadsMonitor(
        AzWorkloadsMonitorProviderInstance providerInstance,
        AzWorkloadsMonitorSapLandscapeMonitor sapLandscapeMonitor,
        ICommand internalCommand
    )
    {
        ProviderInstance = providerInstance;
        SapLandscapeMonitor = sapLandscapeMonitor;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWorkloadsMonitorProviderInstance ProviderInstance { get; }

    public AzWorkloadsMonitorSapLandscapeMonitor SapLandscapeMonitor { get; }

    public async Task<CommandResult> Create(AzWorkloadsMonitorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzWorkloadsMonitorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsMonitorDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzWorkloadsMonitorListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsMonitorListOptions(), token);
    }

    public async Task<CommandResult> Show(AzWorkloadsMonitorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsMonitorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzWorkloadsMonitorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsMonitorUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzWorkloadsMonitorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWorkloadsMonitorWaitOptions(), token);
    }
}