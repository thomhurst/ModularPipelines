using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "delete-publishing-destination")]
public record AwsGuarddutyDeletePublishingDestinationOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--destination-id")] string DestinationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}