using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logicapp", "scale")]
public record AzLogicappScaleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--max-instances")]
    public string? MaxInstances { get; set; }

    [CommandSwitch("--min-instances")]
    public string? MinInstances { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }
}