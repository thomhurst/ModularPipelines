using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "backup", "start")]
public record AzKeyvaultBackupStartOptions : AzOptions
{
    [CliOption("--blob-container-name")]
    public string? BlobContainerName { get; set; }

    [CliOption("--hsm-name")]
    public string? HsmName { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--storage-account-name")]
    public int? StorageAccountName { get; set; }

    [CliOption("--storage-container-SAS-token")]
    public string? StorageContainerSASToken { get; set; }

    [CliOption("--storage-resource-uri")]
    public string? StorageResourceUri { get; set; }

    [CliFlag("--use-managed-identity")]
    public bool? UseManagedIdentity { get; set; }
}