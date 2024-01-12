using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "model-monitoring-jobs", "create")]
public record GcloudAiModelMonitoringJobsCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--emails")] string[] Emails,
[property: CommandSwitch("--endpoint")] string Endpoint,
[property: CommandSwitch("--prediction-sampling-rate")] string PredictionSamplingRate
) : GcloudOptions
{
    [CommandSwitch("--analysis-instance-schema")]
    public string? AnalysisInstanceSchema { get; set; }

    [CommandSwitch("--[no-]anomaly-cloud-logging")]
    public string[]? NoAnomalyCloudLogging { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--log-ttl")]
    public string? LogTtl { get; set; }

    [CommandSwitch("--monitoring-frequency")]
    public string? MonitoringFrequency { get; set; }

    [CommandSwitch("--notification-channels")]
    public string[]? NotificationChannels { get; set; }

    [CommandSwitch("--predict-instance-schema")]
    public string? PredictInstanceSchema { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--sample-predict-request")]
    public string? SamplePredictRequest { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }

    [CommandSwitch("--monitoring-config-from-file")]
    public string? MonitoringConfigFromFile { get; set; }

    [CommandSwitch("--feature-attribution-thresholds")]
    public IEnumerable<KeyValue>? FeatureAttributionThresholds { get; set; }

    [CommandSwitch("--feature-thresholds")]
    public IEnumerable<KeyValue>? FeatureThresholds { get; set; }

    [CommandSwitch("--target-field")]
    public string? TargetField { get; set; }

    [CommandSwitch("--training-sampling-rate")]
    public string? TrainingSamplingRate { get; set; }

    [CommandSwitch("--bigquery-uri")]
    public string? BigqueryUri { get; set; }

    [CommandSwitch("--dataset")]
    public string? Dataset { get; set; }

    [CommandSwitch("--data-format")]
    public string? DataFormat { get; set; }

    [CommandSwitch("--gcs-uris")]
    public string[]? GcsUris { get; set; }
}