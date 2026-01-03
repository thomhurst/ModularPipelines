using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "log-analytics", "workspace")]
public class AzMonitorLogAnalyticsWorkspaceTable
{
    public AzMonitorLogAnalyticsWorkspaceTable(
        AzMonitorLogAnalyticsWorkspaceTableRestore restore,
        AzMonitorLogAnalyticsWorkspaceTableSearchJob searchJob,
        ICommand internalCommand
    )
    {
        Restore = restore;
        SearchJob = searchJob;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorLogAnalyticsWorkspaceTableRestore Restore { get; }

    public AzMonitorLogAnalyticsWorkspaceTableSearchJob SearchJob { get; }

    public async Task<CommandResult> Create(AzMonitorLogAnalyticsWorkspaceTableCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMonitorLogAnalyticsWorkspaceTableDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceTableDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMonitorLogAnalyticsWorkspaceTableListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Migrate(AzMonitorLogAnalyticsWorkspaceTableMigrateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceTableMigrateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMonitorLogAnalyticsWorkspaceTableShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceTableShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMonitorLogAnalyticsWorkspaceTableUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzMonitorLogAnalyticsWorkspaceTableWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceTableWaitOptions(), cancellationToken: token);
    }
}