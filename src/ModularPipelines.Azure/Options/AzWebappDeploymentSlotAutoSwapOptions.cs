using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "deployment", "slot", "auto-swap")]
public record AzWebappDeploymentSlotAutoSwapOptions(
[property: CommandSwitch("--slot")] string Slot
) : AzOptions
{
    [CommandSwitch("--auto-swap-slot")]
    public string? AutoSwapSlot { get; set; }

    [BooleanCommandSwitch("--disable")]
    public bool? Disable { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}