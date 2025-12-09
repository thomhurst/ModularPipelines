using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "get-traffic-policy")]
public record AwsRoute53GetTrafficPolicyOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}