using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "flexible-server", "update")]
public record AzPostgresFlexibleServerUpdateOptions : AzOptions
{
    [CliOption("--active-directory-auth")]
    public string? ActiveDirectoryAuth { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--backup-identity")]
    public string? BackupIdentity { get; set; }

    [CliOption("--backup-key")]
    public string? BackupKey { get; set; }

    [CliOption("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--high-availability")]
    public string? HighAvailability { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--password-auth")]
    public string? PasswordAuth { get; set; }

    [CliOption("--performance-tier")]
    public string? PerformanceTier { get; set; }

    [CliOption("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--standby-zone")]
    public string? StandbyZone { get; set; }

    [CliOption("--storage-auto-grow")]
    public string? StorageAutoGrow { get; set; }

    [CliOption("--storage-size")]
    public string? StorageSize { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}