using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "update-discovery-job")]
public record AwsDatasyncUpdateDiscoveryJobOptions(
[property: CliOption("--discovery-job-arn")] string DiscoveryJobArn,
[property: CliOption("--collection-duration-minutes")] int CollectionDurationMinutes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}