using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stack-hci-vm", "disk", "create")]
public record AzStackHciVmDiskCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--block-size-bytes")]
    public string? BlockSizeBytes { get; set; }

    [CliOption("--disk-file-format")]
    public string? DiskFileFormat { get; set; }

    [CliFlag("--dynamic")]
    public bool? Dynamic { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logical-sector-bytes")]
    public string? LogicalSectorBytes { get; set; }

    [CliOption("--physical-sector-bytes")]
    public string? PhysicalSectorBytes { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--size-gb")]
    public string? SizeGb { get; set; }

    [CliOption("--storage-path-id")]
    public string? StoragePathId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}