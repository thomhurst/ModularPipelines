using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hybridaks", "vnet", "create")]
public record AzHybridaksVnetCreateOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--aods-vnet-id")]
    public string? AodsVnetId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--moc-group")]
    public string? MocGroup { get; set; }

    [CommandSwitch("--moc-location")]
    public string? MocLocation { get; set; }

    [CommandSwitch("--moc-vnet-name")]
    public string? MocVnetName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vsphere-segment-name")]
    public string? VsphereSegmentName { get; set; }
}

