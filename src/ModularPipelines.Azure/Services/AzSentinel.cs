using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzSentinel
{
    public AzSentinel(
        AzSentinelAlertRule alertRule,
        AzSentinelAnalyticsSetting analyticsSetting,
        AzSentinelAutomationRule automationRule,
        AzSentinelBookmark bookmark,
        AzSentinelDataConnector dataConnector,
        AzSentinelEnrichment enrichment,
        AzSentinelEntityQuery entityQuery,
        AzSentinelIncident incident,
        AzSentinelMetadata metadata,
        AzSentinelOfficeConsent officeConsent,
        AzSentinelOnboardingState onboardingState,
        AzSentinelSetting setting,
        AzSentinelSourceControl sourceControl,
        AzSentinelThreatIndicator threatIndicator,
        AzSentinelWatchlist watchlist
    )
    {
        AlertRule = alertRule;
        AnalyticsSetting = analyticsSetting;
        AutomationRule = automationRule;
        Bookmark = bookmark;
        DataConnector = dataConnector;
        Enrichment = enrichment;
        EntityQuery = entityQuery;
        Incident = incident;
        Metadata = metadata;
        OfficeConsent = officeConsent;
        OnboardingState = onboardingState;
        Setting = setting;
        SourceControl = sourceControl;
        ThreatIndicator = threatIndicator;
        Watchlist = watchlist;
    }

    public AzSentinelAlertRule AlertRule { get; }

    public AzSentinelAnalyticsSetting AnalyticsSetting { get; }

    public AzSentinelAutomationRule AutomationRule { get; }

    public AzSentinelBookmark Bookmark { get; }

    public AzSentinelDataConnector DataConnector { get; }

    public AzSentinelEnrichment Enrichment { get; }

    public AzSentinelEntityQuery EntityQuery { get; }

    public AzSentinelIncident Incident { get; }

    public AzSentinelMetadata Metadata { get; }

    public AzSentinelOfficeConsent OfficeConsent { get; }

    public AzSentinelOnboardingState OnboardingState { get; }

    public AzSentinelSetting Setting { get; }

    public AzSentinelSourceControl SourceControl { get; }

    public AzSentinelThreatIndicator ThreatIndicator { get; }

    public AzSentinelWatchlist Watchlist { get; }
}