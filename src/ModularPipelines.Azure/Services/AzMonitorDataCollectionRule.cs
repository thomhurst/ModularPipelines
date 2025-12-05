using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "data-collection")]
public class AzMonitorDataCollectionRule
{
    public AzMonitorDataCollectionRule(
        AzMonitorDataCollectionRuleAssociation association,
        AzMonitorDataCollectionRuleDataFlow dataFlow,
        AzMonitorDataCollectionRuleLogAnalytics logAnalytics,
        AzMonitorDataCollectionRulePerformanceCounter performanceCounter,
        AzMonitorDataCollectionRuleSyslog syslog,
        AzMonitorDataCollectionRuleWindowsEventLog windowsEventLog,
        ICommand internalCommand
    )
    {
        Association = association;
        DataFlow = dataFlow;
        LogAnalytics = logAnalytics;
        PerformanceCounter = performanceCounter;
        Syslog = syslog;
        WindowsEventLog = windowsEventLog;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorDataCollectionRuleAssociation Association { get; }

    public AzMonitorDataCollectionRuleDataFlow DataFlow { get; }

    public AzMonitorDataCollectionRuleLogAnalytics LogAnalytics { get; }

    public AzMonitorDataCollectionRulePerformanceCounter PerformanceCounter { get; }

    public AzMonitorDataCollectionRuleSyslog Syslog { get; }

    public AzMonitorDataCollectionRuleWindowsEventLog WindowsEventLog { get; }

    public async Task<CommandResult> Create(AzMonitorDataCollectionRuleCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMonitorDataCollectionRuleDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionRuleDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMonitorDataCollectionRuleListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionRuleListOptions(), token);
    }

    public async Task<CommandResult> Show(AzMonitorDataCollectionRuleShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionRuleShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMonitorDataCollectionRuleUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMonitorDataCollectionRuleUpdateOptions(), token);
    }
}