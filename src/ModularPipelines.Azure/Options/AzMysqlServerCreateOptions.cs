using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "server", "create")]
public record AzMysqlServerCreateOptions : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-user")]
    public string? AdminUser { get; set; }

    [CliFlag("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CliOption("--auto-grow")]
    public string? AutoGrow { get; set; }

    [CliOption("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CliOption("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CliOption("--infrastructure-encryption")]
    public string? InfrastructureEncryption { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--minimal-tls-version")]
    public string? MinimalTlsVersion { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--public")]
    public string? Public { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--ssl-enforcement")]
    public string? SslEnforcement { get; set; }

    [CliOption("--storage-size")]
    public string? StorageSize { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}