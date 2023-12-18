using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "update-msi-permissions")]
public record AzDataprotectionBackupInstanceUpdateMsiPermissionsOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType,
[property: CommandSwitch("--operation")] string Operation,
[property: CommandSwitch("--permissions-scope")] string PermissionsScope,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-instance")]
    public string? BackupInstance { get; set; }

    [CommandSwitch("--keyvault-id")]
    public string? KeyvaultId { get; set; }

    [CommandSwitch("--restore-request-object")]
    public string? RestoreRequestObject { get; set; }

    [CommandSwitch("--snapshot-resource-group-id")]
    public string? SnapshotResourceGroupId { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}