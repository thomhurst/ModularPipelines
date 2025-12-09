using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "put-alarm")]
public record AwsLightsailPutAlarmOptions(
[property: CliOption("--alarm-name")] string AlarmName,
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--monitored-resource-name")] string MonitoredResourceName,
[property: CliOption("--comparison-operator")] string ComparisonOperator,
[property: CliOption("--threshold")] double Threshold,
[property: CliOption("--evaluation-periods")] int EvaluationPeriods
) : AwsOptions
{
    [CliOption("--datapoints-to-alarm")]
    public int? DatapointsToAlarm { get; set; }

    [CliOption("--treat-missing-data")]
    public string? TreatMissingData { get; set; }

    [CliOption("--contact-protocols")]
    public string[]? ContactProtocols { get; set; }

    [CliOption("--notification-triggers")]
    public string[]? NotificationTriggers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}