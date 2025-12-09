using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "get-findings")]
public record AwsGuarddutyGetFindingsOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--finding-ids")] string[] FindingIds
) : AwsOptions
{
    [CliOption("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}