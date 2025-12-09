using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "autoscale", "profile", "create")]
public record AzMonitorAutoscaleProfileCreateOptions(
[property: CliOption("--autoscale-name")] string AutoscaleName,
[property: CliOption("--count")] int Count,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--timezone")] string Timezone
) : AzOptions
{
    [CliOption("--copy-rules")]
    public string? CopyRules { get; set; }

    [CliOption("--end")]
    public string? End { get; set; }

    [CliOption("--max-count")]
    public int? MaxCount { get; set; }

    [CliOption("--min-count")]
    public int? MinCount { get; set; }

    [CliOption("--recurrence")]
    public string? Recurrence { get; set; }

    [CliOption("--start")]
    public string? Start { get; set; }
}