using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "timer", "update")]
public record AzAcrTaskTimerUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--timer-name")] string TimerName
) : AzOptions
{
    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }
}