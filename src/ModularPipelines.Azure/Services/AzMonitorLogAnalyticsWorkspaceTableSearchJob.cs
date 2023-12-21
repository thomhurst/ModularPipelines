using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "workspace", "table")]
public class AzMonitorLogAnalyticsWorkspaceTableSearchJob
{
    public AzMonitorLogAnalyticsWorkspaceTableSearchJob(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Cancel(AzMonitorLogAnalyticsWorkspaceTableSearchJobCancelOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsWorkspaceTableSearchJobCancelOptions(), token);
    }

    public async Task<CommandResult> Create(AzMonitorLogAnalyticsWorkspaceTableSearchJobCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}