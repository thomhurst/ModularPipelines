using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("change-analysis", "list")]
public record AzChangeAnalysisListOptions(
[property: CommandSwitch("--end-time")] string EndTime,
[property: CommandSwitch("--start-time")] string StartTime
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}