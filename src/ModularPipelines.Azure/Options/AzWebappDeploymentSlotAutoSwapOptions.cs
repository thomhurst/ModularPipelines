using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "deployment", "slot", "auto-swap")]
public record AzWebappDeploymentSlotAutoSwapOptions(
[property: CliOption("--slot")] string Slot
) : AzOptions
{
    [CliOption("--auto-swap-slot")]
    public string? AutoSwapSlot { get; set; }

    [CliFlag("--disable")]
    public bool? Disable { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}