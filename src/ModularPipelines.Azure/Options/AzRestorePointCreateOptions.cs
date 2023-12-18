using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("restore-point", "create")]
public record AzRestorePointCreateOptions(
[property: CommandSwitch("--collection-name")] string CollectionName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--consistency-mode")]
    public string? ConsistencyMode { get; set; }

    [CommandSwitch("--data-disk-restore-point-encryption-set")]
    public string? DataDiskRestorePointEncryptionSet { get; set; }

    [CommandSwitch("--data-disk-restore-point-encryption-type")]
    public string? DataDiskRestorePointEncryptionType { get; set; }

    [CommandSwitch("--exclude-disks")]
    public string? ExcludeDisks { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--os-restore-point-encryption-set")]
    public string? OsRestorePointEncryptionSet { get; set; }

    [CommandSwitch("--os-restore-point-encryption-type")]
    public string? OsRestorePointEncryptionType { get; set; }

    [CommandSwitch("--source-data-disk-resource")]
    public string? SourceDataDiskResource { get; set; }

    [CommandSwitch("--source-os-resource")]
    public string? SourceOsResource { get; set; }

    [CommandSwitch("--source-restore-point")]
    public string? SourceRestorePoint { get; set; }
}