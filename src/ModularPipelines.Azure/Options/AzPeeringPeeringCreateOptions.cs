using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "peering", "create")]
public record AzPeeringPeeringCreateOptions(
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--direct")]
    public string? Direct { get; set; }

    [CommandSwitch("--exchange")]
    public string? Exchange { get; set; }

    [CommandSwitch("--peering-location")]
    public string? PeeringLocation { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}