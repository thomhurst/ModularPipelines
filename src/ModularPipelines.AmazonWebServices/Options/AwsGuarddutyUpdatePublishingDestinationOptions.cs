using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "update-publishing-destination")]
public record AwsGuarddutyUpdatePublishingDestinationOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--destination-id")] string DestinationId
) : AwsOptions
{
    [CliOption("--destination-properties")]
    public string? DestinationProperties { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}