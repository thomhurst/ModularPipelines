using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "key", "restore")]
public record AzKeyvaultKeyRestoreOptions(
[property: CommandSwitch("--algorithm")] string Algorithm,
[property: CommandSwitch("--digest")] string Digest
) : AzOptions
{
    [CommandSwitch("--backup-folder")]
    public string? BackupFolder { get; set; }

    [CommandSwitch("--blob-container-name")]
    public string? BlobContainerName { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--storage-account-name")]
    public int? StorageAccountName { get; set; }

    [CommandSwitch("--storage-container-SAS-token")]
    public string? StorageContainerSASToken { get; set; }

    [CommandSwitch("--storage-resource-uri")]
    public string? StorageResourceUri { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}