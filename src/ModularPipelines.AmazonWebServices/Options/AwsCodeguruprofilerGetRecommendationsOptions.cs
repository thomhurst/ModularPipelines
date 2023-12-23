using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "get-recommendations")]
public record AwsCodeguruprofilerGetRecommendationsOptions(
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName,
[property: CommandSwitch("--start-time")] long StartTime
) : AwsOptions
{
    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}