using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "put-alarm")]
public record AwsLightsailPutAlarmOptions(
[property: CommandSwitch("--alarm-name")] string AlarmName,
[property: CommandSwitch("--metric-name")] string MetricName,
[property: CommandSwitch("--monitored-resource-name")] string MonitoredResourceName,
[property: CommandSwitch("--comparison-operator")] string ComparisonOperator,
[property: CommandSwitch("--threshold")] double Threshold,
[property: CommandSwitch("--evaluation-periods")] int EvaluationPeriods
) : AwsOptions
{
    [CommandSwitch("--datapoints-to-alarm")]
    public int? DatapointsToAlarm { get; set; }

    [CommandSwitch("--treat-missing-data")]
    public string? TreatMissingData { get; set; }

    [CommandSwitch("--contact-protocols")]
    public string[]? ContactProtocols { get; set; }

    [CommandSwitch("--notification-triggers")]
    public string[]? NotificationTriggers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}