using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "list-profile-times")]
public record AwsCodeguruprofilerListProfileTimesOptions(
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--period")] string Period,
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName,
[property: CommandSwitch("--start-time")] long StartTime
) : AwsOptions
{
    [CommandSwitch("--order-by")]
    public string? OrderBy { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}