using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "get-recommendations")]
public record AwsCodeguruprofilerGetRecommendationsOptions(
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--profiling-group-name")] string ProfilingGroupName,
[property: CliOption("--start-time")] long StartTime
) : AwsOptions
{
    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}