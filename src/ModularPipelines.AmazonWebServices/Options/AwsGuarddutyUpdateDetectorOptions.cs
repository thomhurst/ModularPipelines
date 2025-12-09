using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "update-detector")]
public record AwsGuarddutyUpdateDetectorOptions(
[property: CliOption("--detector-id")] string DetectorId
) : AwsOptions
{
    [CliOption("--finding-publishing-frequency")]
    public string? FindingPublishingFrequency { get; set; }

    [CliOption("--data-sources")]
    public string? DataSources { get; set; }

    [CliOption("--features")]
    public string[]? Features { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}