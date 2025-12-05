using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "delete-traffic-policy")]
public record AwsRoute53DeleteTrafficPolicyOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}