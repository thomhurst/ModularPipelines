using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "backup", "start")]
public record AzKeyvaultBackupStartOptions : AzOptions
{
    [CommandSwitch("--blob-container-name")]
    public string? BlobContainerName { get; set; }

    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--storage-account-name")]
    public int? StorageAccountName { get; set; }

    [CommandSwitch("--storage-container-SAS-token")]
    public string? StorageContainerSASToken { get; set; }

    [CommandSwitch("--storage-resource-uri")]
    public string? StorageResourceUri { get; set; }

    [BooleanCommandSwitch("--use-managed-identity")]
    public bool? UseManagedIdentity { get; set; }
}