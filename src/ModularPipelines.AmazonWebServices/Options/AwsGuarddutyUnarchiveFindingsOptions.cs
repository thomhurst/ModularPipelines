using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "unarchive-findings")]
public record AwsGuarddutyUnarchiveFindingsOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--finding-ids")] string[] FindingIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}