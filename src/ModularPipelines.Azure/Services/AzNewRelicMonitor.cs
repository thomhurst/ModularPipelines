using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("new-relic")]
public class AzNewRelicMonitor
{
    public AzNewRelicMonitor(
        AzNewRelicMonitorTagRule tagRule,
        ICommand internalCommand
    )
    {
        TagRule = tagRule;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNewRelicMonitorTagRule TagRule { get; }

    public async Task<CommandResult> Create(AzNewRelicMonitorCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNewRelicMonitorDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMetricRule(AzNewRelicMonitorGetMetricRuleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMetricStatu(AzNewRelicMonitorGetMetricStatuOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNewRelicMonitorListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNewRelicMonitorListOptions(), token);
    }

    public async Task<CommandResult> ListAppService(AzNewRelicMonitorListAppServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListHost(AzNewRelicMonitorListHostOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> MonitoredResource(AzNewRelicMonitorMonitoredResourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNewRelicMonitorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNewRelicMonitorShowOptions(), token);
    }

    public async Task<CommandResult> SwitchBilling(AzNewRelicMonitorSwitchBillingOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VmHostPayload(AzNewRelicMonitorVmHostPayloadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNewRelicMonitorVmHostPayloadOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNewRelicMonitorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNewRelicMonitorWaitOptions(), token);
    }
}