using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyvault", "restore", "start")]
public record AzKeyvaultRestoreStartOptions(
[property: CliOption("--backup-folder")] string BackupFolder
) : AzOptions
{
    [CliOption("--blob-container-name")]
    public string? BlobContainerName { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--storage-account-name")]
    public int? StorageAccountName { get; set; }

    [CliOption("--storage-container-SAS-token")]
    public string? StorageContainerSASToken { get; set; }

    [CliOption("--storage-resource-uri")]
    public string? StorageResourceUri { get; set; }

    [CliFlag("--use-managed-identity")]
    public bool? UseManagedIdentity { get; set; }
}