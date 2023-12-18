using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "metrics")]
public class AzMonitorMetricsAlert
{
    public AzMonitorMetricsAlert(
        AzMonitorMetricsAlertCondition condition,
        AzMonitorMetricsAlertDimension dimension,
        ICommand internalCommand
    )
    {
        Condition = condition;
        Dimension = dimension;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorMetricsAlertCondition Condition { get; }

    public AzMonitorMetricsAlertDimension Dimension { get; }

    public async Task<CommandResult> Create(AzMonitorMetricsAlertCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorMetricsAlertDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorMetricsAlertDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorMetricsAlertListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorMetricsAlertListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorMetricsAlertShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorMetricsAlertShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorMetricsAlertUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorMetricsAlertUpdateOptions(), token);
    }
}