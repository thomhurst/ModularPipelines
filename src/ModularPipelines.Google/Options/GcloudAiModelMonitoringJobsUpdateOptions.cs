using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "model-monitoring-jobs", "update")]
public record GcloudAiModelMonitoringJobsUpdateOptions(
[property: CliArgument] string MonitoringJob,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--analysis-instance-schema")]
    public string? AnalysisInstanceSchema { get; set; }

    [CliOption("--[no-]anomaly-cloud-logging")]
    public string[]? NoAnomalyCloudLogging { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--emails")]
    public string[]? Emails { get; set; }

    [CliOption("--log-ttl")]
    public string? LogTtl { get; set; }

    [CliOption("--monitoring-frequency")]
    public string? MonitoringFrequency { get; set; }

    [CliOption("--notification-channels")]
    public string[]? NotificationChannels { get; set; }

    [CliOption("--prediction-sampling-rate")]
    public string? PredictionSamplingRate { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--monitoring-config-from-file")]
    public string? MonitoringConfigFromFile { get; set; }

    [CliOption("--feature-attribution-thresholds")]
    public IEnumerable<KeyValue>? FeatureAttributionThresholds { get; set; }

    [CliOption("--feature-thresholds")]
    public IEnumerable<KeyValue>? FeatureThresholds { get; set; }
}