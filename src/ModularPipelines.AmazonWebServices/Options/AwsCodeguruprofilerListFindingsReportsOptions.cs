using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "list-findings-reports")]
public record AwsCodeguruprofilerListFindingsReportsOptions(
[property: CommandSwitch("--end-time")] long EndTime,
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName,
[property: CommandSwitch("--start-time")] long StartTime
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}