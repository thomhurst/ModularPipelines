using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "task", "timer", "remove")]
public record AzAcrTaskTimerRemoveOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--timer-name")] string TimerName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}