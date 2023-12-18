using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics")]
public class AzMonitorLogAnalyticsQueryPack
{
    public AzMonitorLogAnalyticsQueryPack(
        AzMonitorLogAnalyticsQueryPackQuery query,
        ICommand internalCommand
    )
    {
        Query = query;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorLogAnalyticsQueryPackQuery Query { get; }

    public async Task<CommandResult> Create(AzMonitorLogAnalyticsQueryPackCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorLogAnalyticsQueryPackDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsQueryPackDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorLogAnalyticsQueryPackListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsQueryPackListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorLogAnalyticsQueryPackShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsQueryPackShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorLogAnalyticsQueryPackUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorLogAnalyticsQueryPackUpdateOptions(), token);
    }
}