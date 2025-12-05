using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "list-profile-times")]
public record AwsCodeguruprofilerListProfileTimesOptions(
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--period")] string Period,
[property: CliOption("--profiling-group-name")] string ProfilingGroupName,
[property: CliOption("--start-time")] long StartTime
) : AwsOptions
{
    [CliOption("--order-by")]
    public string? OrderBy { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}