using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "asset", "query")]
public record AzIotOpsAssetQueryOptions : AzOptions
{
    [CliOption("--asset-type")]
    public string? AssetType { get; set; }

    [CliOption("--cl")]
    public string? Cl { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--documentation-uri")]
    public string? DocumentationUri { get; set; }

    [CliOption("--eai")]
    public string? Eai { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--hardware-revision")]
    public string? HardwareRevision { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--manufacturer")]
    public string? Manufacturer { get; set; }

    [CliOption("--manufacturer-uri")]
    public string? ManufacturerUri { get; set; }

    [CliOption("--model")]
    public string? Model { get; set; }

    [CliOption("--pc")]
    public string? Pc { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }

    [CliOption("--software-revision")]
    public string? SoftwareRevision { get; set; }
}