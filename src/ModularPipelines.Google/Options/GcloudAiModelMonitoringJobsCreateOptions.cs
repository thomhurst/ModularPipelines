using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "model-monitoring-jobs", "create")]
public record GcloudAiModelMonitoringJobsCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--emails")] string[] Emails,
[property: CliOption("--endpoint")] string Endpoint,
[property: CliOption("--prediction-sampling-rate")] string PredictionSamplingRate
) : GcloudOptions
{
    [CliOption("--analysis-instance-schema")]
    public string? AnalysisInstanceSchema { get; set; }

    [CliOption("--[no-]anomaly-cloud-logging")]
    public string[]? NoAnomalyCloudLogging { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--log-ttl")]
    public string? LogTtl { get; set; }

    [CliOption("--monitoring-frequency")]
    public string? MonitoringFrequency { get; set; }

    [CliOption("--notification-channels")]
    public string[]? NotificationChannels { get; set; }

    [CliOption("--predict-instance-schema")]
    public string? PredictInstanceSchema { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--sample-predict-request")]
    public string? SamplePredictRequest { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }

    [CliOption("--monitoring-config-from-file")]
    public string? MonitoringConfigFromFile { get; set; }

    [CliOption("--feature-attribution-thresholds")]
    public IEnumerable<KeyValue>? FeatureAttributionThresholds { get; set; }

    [CliOption("--feature-thresholds")]
    public IEnumerable<KeyValue>? FeatureThresholds { get; set; }

    [CliOption("--target-field")]
    public string? TargetField { get; set; }

    [CliOption("--training-sampling-rate")]
    public string? TrainingSamplingRate { get; set; }

    [CliOption("--bigquery-uri")]
    public string? BigqueryUri { get; set; }

    [CliOption("--dataset")]
    public string? Dataset { get; set; }

    [CliOption("--data-format")]
    public string? DataFormat { get; set; }

    [CliOption("--gcs-uris")]
    public string[]? GcsUris { get; set; }
}