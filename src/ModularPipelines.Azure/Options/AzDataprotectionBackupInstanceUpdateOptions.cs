using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-instance", "update")]
public record AzDataprotectionBackupInstanceUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--backup-instance-name")]
    public string? BackupInstanceName { get; set; }

    [CommandSwitch("--data-source-info")]
    public string? DataSourceInfo { get; set; }

    [CommandSwitch("--data-source-set-info")]
    public string? DataSourceSetInfo { get; set; }

    [CommandSwitch("--datasource-auth-credentials")]
    public string? DatasourceAuthCredentials { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CommandSwitch("--identity-details")]
    public string? IdentityDetails { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--object-type")]
    public string? ObjectType { get; set; }

    [CommandSwitch("--policy-info")]
    public string? PolicyInfo { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--validation-type")]
    public string? ValidationType { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}