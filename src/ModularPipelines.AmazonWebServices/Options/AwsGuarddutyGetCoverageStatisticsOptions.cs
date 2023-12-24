using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "get-coverage-statistics")]
public record AwsGuarddutyGetCoverageStatisticsOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--statistics-type")] string[] StatisticsType
) : AwsOptions
{
    [CommandSwitch("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}