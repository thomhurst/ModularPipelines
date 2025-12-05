using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "get-findings-statistics")]
public record AwsGuarddutyGetFindingsStatisticsOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--finding-statistic-types")] string[] FindingStatisticTypes
) : AwsOptions
{
    [CliOption("--finding-criteria")]
    public string? FindingCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}