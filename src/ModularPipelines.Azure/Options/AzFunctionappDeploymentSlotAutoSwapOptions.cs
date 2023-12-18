using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "deployment", "slot", "auto-swap")]
public record AzFunctionappDeploymentSlotAutoSwapOptions(
[property: CommandSwitch("--slot")] string Slot
) : AzOptions
{
    [CommandSwitch("--auto-swap-slot")]
    public string? AutoSwapSlot { get; set; }

    [CommandSwitch("--disable")]
    public string? Disable { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

