using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "up")]
public record AzMysqlUpOptions : AzOptions
{
    [CliOption("--admin-password")]
    public string? AdminPassword { get; set; }

    [CliOption("--admin-user")]
    public string? AdminUser { get; set; }

    [CliOption("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CliOption("--database-name")]
    public string? DatabaseName { get; set; }

    [CliOption("--generate-password")]
    public string? GeneratePassword { get; set; }

    [CliOption("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server-name")]
    public string? ServerName { get; set; }

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