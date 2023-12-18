using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redis", "create")]
public record AzRedisCreateOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku,
[property: CommandSwitch("--vm-size")] string VmSize
) : AzOptions
{
    [BooleanCommandSwitch("--enable-non-ssl-port")]
    public bool? EnableNonSslPort { get; set; }

    [CommandSwitch("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CommandSwitch("--redis-configuration")]
    public string? RedisConfiguration { get; set; }

    [CommandSwitch("--redis-version")]
    public string? RedisVersion { get; set; }

    [CommandSwitch("--replicas-per-master")]
    public string? ReplicasPerMaster { get; set; }

    [CommandSwitch("--shard-count")]
    public int? ShardCount { get; set; }

    [CommandSwitch("--static-ip")]
    public string? StaticIp { get; set; }

    [CommandSwitch("--subnet-id")]
    public string? SubnetId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tenant-settings")]
    public string? TenantSettings { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}