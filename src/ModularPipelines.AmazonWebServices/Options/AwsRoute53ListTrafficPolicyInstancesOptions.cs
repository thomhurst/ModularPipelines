using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "list-traffic-policy-instances")]
public record AwsRoute53ListTrafficPolicyInstancesOptions : AwsOptions
{
    [CommandSwitch("--hosted-zone-id-marker")]
    public string? HostedZoneIdMarker { get; set; }

    [CommandSwitch("--traffic-policy-instance-name-marker")]
    public string? TrafficPolicyInstanceNameMarker { get; set; }

    [CommandSwitch("--traffic-policy-instance-type-marker")]
    public string? TrafficPolicyInstanceTypeMarker { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}