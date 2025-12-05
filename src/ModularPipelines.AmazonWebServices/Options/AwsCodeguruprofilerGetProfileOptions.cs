using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "get-profile")]
public record AwsCodeguruprofilerGetProfileOptions(
[property: CliOption("--profiling-group-name")] string ProfilingGroupName
) : AwsOptions
{
    [CliOption("--accept")]
    public string? Accept { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--max-depth")]
    public int? MaxDepth { get; set; }

    [CliOption("--period")]
    public string? Period { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }
}