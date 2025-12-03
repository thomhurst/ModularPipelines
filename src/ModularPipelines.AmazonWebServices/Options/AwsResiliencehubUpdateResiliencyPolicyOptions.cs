using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "update-resiliency-policy")]
public record AwsResiliencehubUpdateResiliencyPolicyOptions(
[property: CliOption("--policy-arn")] string PolicyArn
) : AwsOptions
{
    [CliOption("--data-location-constraint")]
    public string? DataLocationConstraint { get; set; }

    [CliOption("--policy")]
    public IEnumerable<KeyValue>? Policy { get; set; }

    [CliOption("--policy-description")]
    public string? PolicyDescription { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}