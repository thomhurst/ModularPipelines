using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "create")]
public record AzImageCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--data-disk-caching")]
    public string? DataDiskCaching { get; set; }

    [CommandSwitch("--data-disk-sources")]
    public string? DataDiskSources { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CommandSwitch("--hyper-v-generation")]
    public string? HyperVGeneration { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--os-disk-caching")]
    public string? OsDiskCaching { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--storage-sku")]
    public string? StorageSku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [BooleanCommandSwitch("--zone-resilient")]
    public bool? ZoneResilient { get; set; }
}

