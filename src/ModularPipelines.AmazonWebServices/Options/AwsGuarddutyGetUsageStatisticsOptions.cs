using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "get-usage-statistics")]
public record AwsGuarddutyGetUsageStatisticsOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--usage-statistic-type")] string UsageStatisticType,
[property: CliOption("--usage-criteria")] string UsageCriteria
) : AwsOptions
{
    [CliOption("--unit")]
    public string? Unit { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}