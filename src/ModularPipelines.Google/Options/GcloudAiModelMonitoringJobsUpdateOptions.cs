using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "model-monitoring-jobs", "update")]
public record GcloudAiModelMonitoringJobsUpdateOptions(
[property: PositionalArgument] string MonitoringJob,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--analysis-instance-schema")]
    public string? AnalysisInstanceSchema { get; set; }

    [CommandSwitch("--[no-]anomaly-cloud-logging")]
    public string[]? NoAnomalyCloudLogging { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--emails")]
    public string[]? Emails { get; set; }

    [CommandSwitch("--log-ttl")]
    public string? LogTtl { get; set; }

    [CommandSwitch("--monitoring-frequency")]
    public string? MonitoringFrequency { get; set; }

    [CommandSwitch("--notification-channels")]
    public string[]? NotificationChannels { get; set; }

    [CommandSwitch("--prediction-sampling-rate")]
    public string? PredictionSamplingRate { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--monitoring-config-from-file")]
    public string? MonitoringConfigFromFile { get; set; }

    [CommandSwitch("--feature-attribution-thresholds")]
    public IEnumerable<KeyValue>? FeatureAttributionThresholds { get; set; }

    [CommandSwitch("--feature-thresholds")]
    public IEnumerable<KeyValue>? FeatureThresholds { get; set; }
}