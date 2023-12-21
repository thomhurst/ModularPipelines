using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace")]
public class AzMonitorLogAnalyticsWorkspaceLinkedStorage
{
    public AzMonitorLogAnalyticsWorkspaceLinkedStorage(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzMonitorLogAnalyticsWorkspaceLinkedStorageAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzMonitorLogAnalyticsWorkspaceLinkedStorageCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorLogAnalyticsWorkspaceLinkedStorageDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceLinkedStorageDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorLogAnalyticsWorkspaceLinkedStorageListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzMonitorLogAnalyticsWorkspaceLinkedStorageRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMonitorLogAnalyticsWorkspaceLinkedStorageShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceLinkedStorageShowOptions(), token);
    }
}