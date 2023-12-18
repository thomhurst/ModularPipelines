using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "asset", "update")]
public record AzIotOpsAssetUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--asset-type")]
    public string? AssetType { get; set; }

    [CommandSwitch("--data-publish-int")]
    public string? DataPublishInt { get; set; }

    [CommandSwitch("--data-queue-size")]
    public string? DataQueueSize { get; set; }

    [CommandSwitch("--data-sample-int")]
    public string? DataSampleInt { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disable")]
    public bool? Disable { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--documentation-uri")]
    public string? DocumentationUri { get; set; }

    [CommandSwitch("--eai")]
    public string? Eai { get; set; }

    [CommandSwitch("--epi")]
    public string? Epi { get; set; }

    [CommandSwitch("--eqs")]
    public string? Eqs { get; set; }

    [CommandSwitch("--esi")]
    public string? Esi { get; set; }

    [CommandSwitch("--hardware-revision")]
    public string? HardwareRevision { get; set; }

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

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}