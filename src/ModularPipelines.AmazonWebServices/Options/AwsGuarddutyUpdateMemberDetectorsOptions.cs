using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "update-member-detectors")]
public record AwsGuarddutyUpdateMemberDetectorsOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CliOption("--data-sources")]
    public string? DataSources { get; set; }

    [CliOption("--features")]
    public string[]? Features { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}