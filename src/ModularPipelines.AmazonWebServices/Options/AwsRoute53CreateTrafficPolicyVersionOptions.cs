using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-traffic-policy-version")]
public record AwsRoute53CreateTrafficPolicyVersionOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--document")] string Document
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}