using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("change-analysis", "list-by-resource")]
public record AzChangeAnalysisListByResourceOptions(
[property: CommandSwitch("--end-time")] string EndTime,
[property: CommandSwitch("--resource")] string Resource,
[property: CommandSwitch("--start-time")] string StartTime
) : AzOptions
{
    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}

