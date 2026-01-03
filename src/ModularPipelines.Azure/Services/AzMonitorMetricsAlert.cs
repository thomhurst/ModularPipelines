using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "metrics")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMonitorMetricsAlertDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorMetricsAlertDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMonitorMetricsAlertListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorMetricsAlertListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMonitorMetricsAlertShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorMetricsAlertShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMonitorMetricsAlertUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorMetricsAlertUpdateOptions(), cancellationToken: token);
    }
}