using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("change-analysis", "list")]
public record AzChangeAnalysisListOptions(
[property: CliOption("--end-time")] string EndTime,
[property: CliOption("--start-time")] string StartTime
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}