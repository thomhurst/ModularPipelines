using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "describe-discovery-job")]
public record AwsDatasyncDescribeDiscoveryJobOptions(
[property: CliOption("--discovery-job-arn")] string DiscoveryJobArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}