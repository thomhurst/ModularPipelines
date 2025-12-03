using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "list-traffic-policy-versions")]
public record AwsRoute53ListTrafficPolicyVersionsOptions(
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--traffic-policy-version-marker")]
    public string? TrafficPolicyVersionMarker { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}