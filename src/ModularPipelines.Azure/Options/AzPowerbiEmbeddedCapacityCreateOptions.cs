using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("powerbi", "embedded-capacity", "create")]
public record AzPowerbiEmbeddedCapacityCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku-name")] string SkuName
) : AzOptions
{
    [CommandSwitch("--administration-members")]
    public string? AdministrationMembers { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sku-tier")]
    public string? SkuTier { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}