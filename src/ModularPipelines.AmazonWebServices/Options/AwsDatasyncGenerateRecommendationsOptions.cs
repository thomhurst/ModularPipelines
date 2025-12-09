using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "generate-recommendations")]
public record AwsDatasyncGenerateRecommendationsOptions(
[property: CliOption("--discovery-job-arn")] string DiscoveryJobArn,
[property: CliOption("--resource-ids")] string[] ResourceIds,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}