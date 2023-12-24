using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-alarms")]
public record AwsLightsailGetAlarmsOptions : AwsOptions
{
    [CommandSwitch("--alarm-name")]
    public string? AlarmName { get; set; }

    [CommandSwitch("--page-token")]
    public string? PageToken { get; set; }

    [CommandSwitch("--monitored-resource-name")]
    public string? MonitoredResourceName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}