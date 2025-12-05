using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "flexible-server", "replica", "create")]
public record AzPostgresFlexibleServerReplicaCreateOptions(
[property: CliOption("--replica-name")] string ReplicaName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source-server")] string SourceServer
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--performance-tier")]
    public string? PerformanceTier { get; set; }

    [CliOption("--private-dns-zone")]
    public string? PrivateDnsZone { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--storage-size")]
    public string? StorageSize { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-prefixes")]
    public string? SubnetPrefixes { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--vnet")]
    public string? Vnet { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}