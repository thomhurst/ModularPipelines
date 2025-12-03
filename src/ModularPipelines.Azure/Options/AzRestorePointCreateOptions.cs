using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("restore-point", "create")]
public record AzRestorePointCreateOptions(
[property: CliOption("--collection-name")] string CollectionName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--consistency-mode")]
    public string? ConsistencyMode { get; set; }

    [CliOption("--data-disk-restore-point-encryption-set")]
    public string? DataDiskRestorePointEncryptionSet { get; set; }

    [CliOption("--data-disk-restore-point-encryption-type")]
    public string? DataDiskRestorePointEncryptionType { get; set; }

    [CliOption("--exclude-disks")]
    public string? ExcludeDisks { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-restore-point-encryption-set")]
    public string? OsRestorePointEncryptionSet { get; set; }

    [CliOption("--os-restore-point-encryption-type")]
    public string? OsRestorePointEncryptionType { get; set; }

    [CliOption("--source-data-disk-resource")]
    public string? SourceDataDiskResource { get; set; }

    [CliOption("--source-os-resource")]
    public string? SourceOsResource { get; set; }

    [CliOption("--source-restore-point")]
    public string? SourceRestorePoint { get; set; }
}