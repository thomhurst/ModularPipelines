using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-instance", "initialize")]
public record AzDataprotectionBackupInstanceInitializeOptions(
[property: CliOption("--datasource-id")] string DatasourceId,
[property: CliOption("--datasource-location")] string DatasourceLocation,
[property: CliOption("--datasource-type")] string DatasourceType,
[property: CliOption("--policy-id")] string PolicyId
) : AzOptions
{
    [CliOption("--backup-configuration")]
    public string? BackupConfiguration { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--secret-store-type")]
    public string? SecretStoreType { get; set; }

    [CliOption("--secret-store-uri")]
    public string? SecretStoreUri { get; set; }

    [CliOption("--snapshot-resource-group-name")]
    public string? SnapshotResourceGroupName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}