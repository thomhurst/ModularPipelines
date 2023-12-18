using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzMonitor
{
    public AzMonitor(
        AzMonitorAccount account,
        AzMonitorActionGroup actionGroup,
        AzMonitorActivityLog activityLog,
        AzMonitorAlertProcessingRule alertProcessingRule,
        AzMonitorAppInsights appInsights,
        AzMonitorAutoscale autoscale,
        AzMonitorDataCollection dataCollection,
        AzMonitorDiagnosticSettings diagnosticSettings,
        AzMonitorLogAnalytics logAnalytics,
        AzMonitorLogProfiles logProfiles,
        AzMonitorMetrics metrics,
        AzMonitorPrivateLinkScope privateLinkScope,
        AzMonitorScheduledQuery scheduledQuery,
        ICommand internalCommand
    )
    {
        Account = account;
        ActionGroup = actionGroup;
        ActivityLog = activityLog;
        AlertProcessingRule = alertProcessingRule;
        AppInsights = appInsights;
        Autoscale = autoscale;
        DataCollection = dataCollection;
        DiagnosticSettings = diagnosticSettings;
        LogAnalytics = logAnalytics;
        LogProfiles = logProfiles;
        Metrics = metrics;
        PrivateLinkScope = privateLinkScope;
        ScheduledQuery = scheduledQuery;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMonitorAccount Account { get; }

    public AzMonitorActionGroup ActionGroup { get; }

    public AzMonitorActivityLog ActivityLog { get; }

    public AzMonitorAlertProcessingRule AlertProcessingRule { get; }

    public AzMonitorAppInsights AppInsights { get; }

    public AzMonitorAutoscale Autoscale { get; }

    public AzMonitorDataCollection DataCollection { get; }

    public AzMonitorDiagnosticSettings DiagnosticSettings { get; }

    public AzMonitorLogAnalytics LogAnalytics { get; }

    public AzMonitorLogProfiles LogProfiles { get; }

    public AzMonitorMetrics Metrics { get; }

    public AzMonitorPrivateLinkScope PrivateLinkScope { get; }

    public AzMonitorScheduledQuery ScheduledQuery { get; }

    public async Task<CommandResult> Clone(AzMonitorCloneOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

