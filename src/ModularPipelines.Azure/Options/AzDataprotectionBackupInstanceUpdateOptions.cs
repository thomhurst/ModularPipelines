using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("dataprotection", "backup-instance", "update")]
public record AzDataprotectionBackupInstanceUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--backup-instance-name")]
    public string? BackupInstanceName { get; set; }

    [CliOption("--data-source-info")]
    public string? DataSourceInfo { get; set; }

    [CliOption("--data-source-set-info")]
    public string? DataSourceSetInfo { get; set; }

    [CliOption("--datasource-auth-credentials")]
    public string? DatasourceAuthCredentials { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--friendly-name")]
    public string? FriendlyName { get; set; }

    [CliOption("--identity-details")]
    public string? IdentityDetails { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--object-type")]
    public string? ObjectType { get; set; }

    [CliOption("--policy-info")]
    public string? PolicyInfo { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--validation-type")]
    public string? ValidationType { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}