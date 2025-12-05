using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "asset", "update")]
public record AzIotOpsAssetUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--asset-type")]
    public string? AssetType { get; set; }

    [CliOption("--data-publish-int")]
    public string? DataPublishInt { get; set; }

    [CliOption("--data-queue-size")]
    public string? DataQueueSize { get; set; }

    [CliOption("--data-sample-int")]
    public string? DataSampleInt { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disable")]
    public bool? Disable { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--documentation-uri")]
    public string? DocumentationUri { get; set; }

    [CliOption("--eai")]
    public string? Eai { get; set; }

    [CliOption("--epi")]
    public string? Epi { get; set; }

    [CliOption("--eqs")]
    public string? Eqs { get; set; }

    [CliOption("--esi")]
    public string? Esi { get; set; }

    [CliOption("--hardware-revision")]
    public string? HardwareRevision { get; set; }

    [CliOption("--manufacturer")]
    public string? Manufacturer { get; set; }

    [CliOption("--manufacturer-uri")]
    public string? ManufacturerUri { get; set; }

    [CliOption("--model")]
    public string? Model { get; set; }

    [CliOption("--pc")]
    public string? Pc { get; set; }

    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }

    [CliOption("--software-revision")]
    public string? SoftwareRevision { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}