using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "virtualharddisk", "create")]
public record AzAzurestackhciVirtualharddiskCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--block-size-bytes")]
    public string? BlockSizeBytes { get; set; }

    [CommandSwitch("--disk-file-format")]
    public string? DiskFileFormat { get; set; }

    [CommandSwitch("--disk-size-gb")]
    public string? DiskSizeGb { get; set; }

    [BooleanCommandSwitch("--dynamic")]
    public bool? Dynamic { get; set; }

    [CommandSwitch("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--logical-sector-bytes")]
    public string? LogicalSectorBytes { get; set; }

    [CommandSwitch("--physical-sector-bytes")]
    public string? PhysicalSectorBytes { get; set; }

    [CommandSwitch("--storagepath-id")]
    public string? StoragepathId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

