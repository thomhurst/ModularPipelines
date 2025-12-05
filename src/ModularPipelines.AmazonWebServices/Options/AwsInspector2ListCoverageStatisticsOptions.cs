using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("inspector2", "list-coverage-statistics")]
public record AwsInspector2ListCoverageStatisticsOptions : AwsOptions
{
    [CliOption("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CliOption("--group-by")]
    public string? GroupBy { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}