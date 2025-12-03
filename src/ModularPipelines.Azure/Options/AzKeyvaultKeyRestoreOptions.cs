using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "key", "restore")]
public record AzKeyvaultKeyRestoreOptions : AzOptions
{
    [CliOption("--backup-folder")]
    public string? BackupFolder { get; set; }

    [CliOption("--blob-container-name")]
    public string? BlobContainerName { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--storage-account-name")]
    public int? StorageAccountName { get; set; }

    [CliOption("--storage-container-SAS-token")]
    public string? StorageContainerSASToken { get; set; }

    [CliOption("--storage-resource-uri")]
    public string? StorageResourceUri { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}