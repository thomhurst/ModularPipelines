using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "list-findings-reports")]
public record AwsCodeguruprofilerListFindingsReportsOptions(
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--profiling-group-name")] string ProfilingGroupName,
[property: CliOption("--start-time")] long StartTime
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}