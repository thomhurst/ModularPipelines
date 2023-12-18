using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "task", "timer", "add")]
public record AzAcrTaskTimerAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--schedule")] string Schedule,
[property: CommandSwitch("--timer-name")] string TimerName
) : AzOptions
{
    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}