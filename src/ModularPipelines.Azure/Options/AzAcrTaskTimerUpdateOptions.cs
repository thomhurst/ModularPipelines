using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "task", "timer", "update")]
public record AzAcrTaskTimerUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--timer-name")] string TimerName
) : AzOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--schedule")]
    public string? Schedule { get; set; }
}