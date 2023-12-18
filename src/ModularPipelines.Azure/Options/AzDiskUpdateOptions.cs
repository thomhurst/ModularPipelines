using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk", "update")]
public record AzDiskUpdateOptions : AzOptions
{
    [BooleanCommandSwitch("--accelerated-network")]
    public bool? AcceleratedNetwork { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--data-access-auth-mode")]
    public string? DataAccessAuthMode { get; set; }

    [CommandSwitch("--disk-access")]
    public string? DiskAccess { get; set; }

    [CommandSwitch("--disk-encryption-set")]
    public string? DiskEncryptionSet { get; set; }

    [CommandSwitch("--disk-iops-read-only")]
    public string? DiskIopsReadOnly { get; set; }

    [CommandSwitch("--disk-iops-read-write")]
    public string? DiskIopsReadWrite { get; set; }

    [CommandSwitch("--disk-mbps-read-only")]
    public string? DiskMbpsReadOnly { get; set; }

    [CommandSwitch("--disk-mbps-read-write")]
    public string? DiskMbpsReadWrite { get; set; }

    [BooleanCommandSwitch("--enable-bursting")]
    public bool? EnableBursting { get; set; }

    [CommandSwitch("--encryption-type")]
    public string? EncryptionType { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--max-shares")]
    public string? MaxShares { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network-access-policy")]
    public string? NetworkAccessPolicy { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--size-gb")]
    public string? SizeGb { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}