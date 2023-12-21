using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzHdinsight
{
    public AzHdinsight(
        AzHdinsightApplication application,
        AzHdinsightAutoscale autoscale,
        AzHdinsightAzureMonitor azureMonitor,
        AzHdinsightHost host,
        AzHdinsightMonitor monitor,
        AzHdinsightScriptAction scriptAction,
        ICommand internalCommand
    )
    {
        Application = application;
        Autoscale = autoscale;
        AzureMonitor = azureMonitor;
        Host = host;
        Monitor = monitor;
        ScriptAction = scriptAction;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzHdinsightApplication Application { get; }

    public AzHdinsightAutoscale Autoscale { get; }

    public AzHdinsightAzureMonitor AzureMonitor { get; }

    public AzHdinsightHost Host { get; }

    public AzHdinsightMonitor Monitor { get; }

    public AzHdinsightScriptAction ScriptAction { get; }

    public async Task<CommandResult> Create(AzHdinsightCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzHdinsightDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzHdinsightListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzHdinsightListOptions(), token);
    }

    public async Task<CommandResult> ListUsage(AzHdinsightListUsageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resize(AzHdinsightResizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RotateDiskEncryptionKey(AzHdinsightRotateDiskEncryptionKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzHdinsightShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzHdinsightUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzHdinsightWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}