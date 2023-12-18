using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci-vm", "disk", "create")]
public record AzStackHciVmDiskCreateOptions(
[property: CommandSwitch("--custom-location")] string CustomLocation,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--block-size-bytes")]
    public string? BlockSizeBytes { get; set; }

    [CommandSwitch("--disk-file-format")]
    public string? DiskFileFormat { get; set; }

    [BooleanCommandSwitch("--dynamic")]
    public bool? Dynamic { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--logical-sector-bytes")]
    public string? LogicalSectorBytes { get; set; }

    [CommandSwitch("--physical-sector-bytes")]
    public string? PhysicalSectorBytes { get; set; }

    [CommandSwitch("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CommandSwitch("--size-gb")]
    public string? SizeGb { get; set; }

    [CommandSwitch("--storage-path-id")]
    public string? StoragePathId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}