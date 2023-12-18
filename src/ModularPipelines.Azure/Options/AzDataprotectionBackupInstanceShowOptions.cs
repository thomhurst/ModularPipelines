using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "show")]
public record AzDataprotectionBackupInstanceShowOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType,
[property: CommandSwitch("--operation")] string Operation,
[property: CommandSwitch("--permissions-scope")] string PermissionsScope,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--backup-instance-name")]
    public string? BackupInstanceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

