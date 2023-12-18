using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "delete")]
public record AzDataprotectionBackupInstanceDeleteOptions(
[property: CommandSwitch("--datasource-id")] string DatasourceId,
[property: CommandSwitch("--datasource-location")] string DatasourceLocation,
[property: CommandSwitch("--datasource-type")] string DatasourceType,
[property: CommandSwitch("--policy-id")] string PolicyId
) : AzOptions
{
    [CommandSwitch("--backup-instance-name")]
    public string? BackupInstanceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}