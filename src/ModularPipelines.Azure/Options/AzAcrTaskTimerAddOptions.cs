using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "task", "timer", "add")]
public record AzAcrTaskTimerAddOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--schedule")] string Schedule,
[property: CliOption("--timer-name")] string TimerName
) : AzOptions
{
    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}