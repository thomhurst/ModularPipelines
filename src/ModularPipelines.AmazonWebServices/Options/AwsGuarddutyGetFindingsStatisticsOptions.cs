using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "get-findings-statistics")]
public record AwsGuarddutyGetFindingsStatisticsOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--finding-statistic-types")] string[] FindingStatisticTypes
) : AwsOptions
{
    [CommandSwitch("--finding-criteria")]
    public string? FindingCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}