using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "update-traffic-policy-comment")]
public record AwsRoute53UpdateTrafficPolicyCommentOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--comment")] string Comment,
[property: CliOption("--traffic-policy-version")] int TrafficPolicyVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}