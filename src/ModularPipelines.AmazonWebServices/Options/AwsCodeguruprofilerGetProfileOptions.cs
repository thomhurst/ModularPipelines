using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "get-profile")]
public record AwsCodeguruprofilerGetProfileOptions(
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CommandSwitch("--accept")]
    public string? Accept { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--max-depth")]
    public int? MaxDepth { get; set; }

    [CommandSwitch("--period")]
    public string? Period { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }
}