using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor")]
public class AzMonitorAutoscale
{
    public AzMonitorAutoscale(
        AzMonitorAutoscaleProfile profile,
        AzMonitorAutoscaleRule rule,
        ICommand internalCommand
    )
    {
        Profile = profile;
        Rule = rule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorAutoscaleProfile Profile { get; }

    public AzMonitorAutoscaleRule Rule { get; }

    public async Task<CommandResult> Create(AzMonitorAutoscaleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorAutoscaleDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzMonitorAutoscaleListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzMonitorAutoscaleShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowPredictiveMetric(AzMonitorAutoscaleShowPredictiveMetricOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzMonitorAutoscaleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorAutoscaleUpdateOptions(), token);
    }
}