using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-traffic-policy-instances-by-policy")]
public record AwsRoute53ListTrafficPolicyInstancesByPolicyOptions(
[property: CliOption("--traffic-policy-id")] string TrafficPolicyId,
[property: CliOption("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CliOption("--hosted-zone-id-marker")]
    public string? HostedZoneIdMarker { get; set; }

    [CliOption("--traffic-policy-instance-name-marker")]
    public string? TrafficPolicyInstanceNameMarker { get; set; }

    [CliOption("--traffic-policy-instance-type-marker")]
    public string? TrafficPolicyInstanceTypeMarker { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}