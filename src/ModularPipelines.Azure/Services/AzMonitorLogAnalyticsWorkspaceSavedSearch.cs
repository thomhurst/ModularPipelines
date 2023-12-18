using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace")]
public class AzMonitorLogAnalyticsWorkspaceSavedSearch
{
    public AzMonitorLogAnalyticsWorkspaceSavedSearch(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzMonitorLogAnalyticsWorkspaceSavedSearchCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorLogAnalyticsWorkspaceSavedSearchDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceSavedSearchDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorLogAnalyticsWorkspaceSavedSearchListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMonitorLogAnalyticsWorkspaceSavedSearchShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceSavedSearchShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorLogAnalyticsWorkspaceSavedSearchUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}