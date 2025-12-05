using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "update-traffic-policy-instance")]
public record AwsRoute53UpdateTrafficPolicyInstanceOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--ttl")] long Ttl,
[property: CliOption("--traffic-policy-id")] string TrafficPolicyId,
[property: CliOption("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}