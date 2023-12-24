using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "get-usage-statistics")]
public record AwsGuarddutyGetUsageStatisticsOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--usage-statistic-type")] string UsageStatisticType,
[property: CommandSwitch("--usage-criteria")] string UsageCriteria
) : AwsOptions
{
    [CommandSwitch("--unit")]
    public string? Unit { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}