using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("image", "create")]
public record AzImageCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--data-disk-caching")]
    public string? DataDiskCaching { get; set; }

    [CliOption("--data-disk-sources")]
    public string? DataDiskSources { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--hyper-v-generation")]
    public string? HyperVGeneration { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--os-disk-caching")]
    public string? OsDiskCaching { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--storage-sku")]
    public string? StorageSku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--zone-resilient")]
    public bool? ZoneResilient { get; set; }
}