using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-instance", "update-msi-permissions")]
public record AzDataprotectionBackupInstanceUpdateMsiPermissionsOptions(
[property: CliOption("--datasource-type")] string DatasourceType,
[property: CliOption("--operation")] string Operation,
[property: CliOption("--permissions-scope")] string PermissionsScope,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--backup-instance")]
    public string? BackupInstance { get; set; }

    [CliOption("--keyvault-id")]
    public string? KeyvaultId { get; set; }

    [CliOption("--restore-request-object")]
    public string? RestoreRequestObject { get; set; }

    [CliOption("--snapshot-resource-group-id")]
    public string? SnapshotResourceGroupId { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}