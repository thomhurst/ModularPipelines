using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "get-alarms")]
public record AwsLightsailGetAlarmsOptions : AwsOptions
{
    [CliOption("--alarm-name")]
    public string? AlarmName { get; set; }

    [CliOption("--page-token")]
    public string? PageToken { get; set; }

    [CliOption("--monitored-resource-name")]
    public string? MonitoredResourceName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}