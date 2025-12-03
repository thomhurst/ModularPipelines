using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redis", "create")]
public record AzRedisCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku,
[property: CliOption("--vm-size")] string VmSize
) : AzOptions
{
    [CliFlag("--enable-non-ssl-port")]
    public bool? EnableNonSslPort { get; set; }

    [CliOption("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CliOption("--redis-configuration")]
    public string? RedisConfiguration { get; set; }

    [CliOption("--redis-version")]
    public string? RedisVersion { get; set; }

    [CliOption("--replicas-per-master")]
    public string? ReplicasPerMaster { get; set; }

    [CliOption("--shard-count")]
    public int? ShardCount { get; set; }

    [CliOption("--static-ip")]
    public string? StaticIp { get; set; }

    [CliOption("--subnet-id")]
    public string? SubnetId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tenant-settings")]
    public string? TenantSettings { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}