using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("datadog")]
public class AzDatadogMonitor
{
    public AzDatadogMonitor(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzDatadogMonitorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzDatadogMonitorDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatadogMonitorDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetDefaultKey(AzDatadogMonitorGetDefaultKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatadogMonitorGetDefaultKeyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDatadogMonitorListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatadogMonitorListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ListApiKey(AzDatadogMonitorListApiKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListHost(AzDatadogMonitorListHostOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListLinkedResource(AzDatadogMonitorListLinkedResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListMonitoredResource(AzDatadogMonitorListMonitoredResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RefreshSetPasswordLink(AzDatadogMonitorRefreshSetPasswordLinkOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatadogMonitorRefreshSetPasswordLinkOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> SetDefaultKey(AzDatadogMonitorSetDefaultKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatadogMonitorSetDefaultKeyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDatadogMonitorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatadogMonitorShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDatadogMonitorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatadogMonitorUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDatadogMonitorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatadogMonitorWaitOptions(), cancellationToken: token);
    }
}