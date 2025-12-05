using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "get-coverage-statistics")]
public record AwsGuarddutyGetCoverageStatisticsOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--statistics-type")] string[] StatisticsType
) : AwsOptions
{
    [CliOption("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}