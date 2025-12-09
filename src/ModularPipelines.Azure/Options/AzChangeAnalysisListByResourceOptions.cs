using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("change-analysis", "list-by-resource")]
public record AzChangeAnalysisListByResourceOptions(
[property: CliOption("--end-time")] string EndTime,
[property: CliOption("--resource")] string Resource,
[property: CliOption("--start-time")] string StartTime
) : AzOptions
{
    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}