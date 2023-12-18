using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "flexible-server", "restore")]
public record AzMysqlFlexibleServerRestoreOptions(
[property: CommandSwitch("--source-server")] string SourceServer
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CommandSwitch("--backup-retention")]
    public string? BackupRetention { get; set; }

    [CommandSwitch("--geo-redundant-backup")]
    public string? GeoRedundantBackup { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CommandSwitch("--public-access")]
    public string? PublicAccess { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--restore-time")]
    public string? RestoreTime { get; set; }

    [CommandSwitch("--sku-name")]
    public string? SkuName { get; set; }

    [CommandSwitch("--storage-auto-grow")]
    public string? StorageAutoGrow { get; set; }

    [CommandSwitch("--storage-size")]
    public string? StorageSize { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--vnet")]
    public string? Vnet { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}