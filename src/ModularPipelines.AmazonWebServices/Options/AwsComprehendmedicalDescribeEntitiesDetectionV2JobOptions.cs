using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehendmedical", "describe-entities-detection-v2-job")]
public record AwsComprehendmedicalDescribeEntitiesDetectionV2JobOptions(
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}