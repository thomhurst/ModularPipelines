using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "asset", "query")]
public record AzIotOpsAssetQueryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--asset-type")]
    public string? AssetType { get; set; }

    [CommandSwitch("--cl")]
    public string? Cl { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--documentation-uri")]
    public string? DocumentationUri { get; set; }

    [CommandSwitch("--eai")]
    public string? Eai { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--hardware-revision")]
    public string? HardwareRevision { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--manufacturer")]
    public string? Manufacturer { get; set; }

    [CommandSwitch("--manufacturer-uri")]
    public string? ManufacturerUri { get; set; }

    [CommandSwitch("--model")]
    public string? Model { get; set; }

    [CommandSwitch("--pc")]
    public string? Pc { get; set; }

    [CommandSwitch("--serial-number")]
    public string? SerialNumber { get; set; }

    [CommandSwitch("--software-revision")]
    public string? SoftwareRevision { get; set; }
}

