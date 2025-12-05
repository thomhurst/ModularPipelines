using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-traffic-policy-instance")]
public record AwsRoute53CreateTrafficPolicyInstanceOptions(
[property: CliOption("--hosted-zone-id")] string HostedZoneId,
[property: CliOption("--name")] string Name,
[property: CliOption("--ttl")] long Ttl,
[property: CliOption("--traffic-policy-id")] string TrafficPolicyId,
[property: CliOption("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}