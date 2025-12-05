using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("urestackhci", "virtualharddisk", "create")]
public record AzAzurestackhciVirtualharddiskCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--block-size-bytes")]
    public string? BlockSizeBytes { get; set; }

    [CliOption("--disk-file-format")]
    public string? DiskFileFormat { get; set; }

    [CliOption("--disk-size-gb")]
    public string? DiskSizeGb { get; set; }

    [CliFlag("--dynamic")]
    public bool? Dynamic { get; set; }

    [CliOption("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logical-sector-bytes")]
    public string? LogicalSectorBytes { get; set; }

    [CliOption("--physical-sector-bytes")]
    public string? PhysicalSectorBytes { get; set; }

    [CliOption("--storagepath-id")]
    public string? StoragepathId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}