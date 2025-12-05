using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "deployment", "slot", "swap")]
public record AzFunctionappDeploymentSlotSwapOptions(
[property: CliOption("--slot")] string Slot
) : AzOptions
{
    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--preserve-vnet")]
    public bool? PreserveVnet { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-slot")]
    public string? TargetSlot { get; set; }
}