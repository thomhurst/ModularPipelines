using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "describe-publishing-destination")]
public record AwsGuarddutyDescribePublishingDestinationOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--destination-id")] string DestinationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}