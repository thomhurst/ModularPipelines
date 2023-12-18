using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redisenterprise", "create")]
public record AzRedisenterpriseCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--assigned-identities")]
    public string? AssignedIdentities { get; set; }

    [CommandSwitch("--capacity")]
    public string? Capacity { get; set; }

    [CommandSwitch("--client-protocol")]
    public string? ClientProtocol { get; set; }

    [CommandSwitch("--clustering-policy")]
    public string? ClusteringPolicy { get; set; }

    [CommandSwitch("--eviction-policy")]
    public string? EvictionPolicy { get; set; }

    [CommandSwitch("--group-nickname")]
    public string? GroupNickname { get; set; }

    [CommandSwitch("--identity-resource-id")]
    public string? IdentityResourceId { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--key-encryption-identity-type")]
    public string? KeyEncryptionIdentityType { get; set; }

    [CommandSwitch("--key-encryption-key-url")]
    public string? KeyEncryptionKeyUrl { get; set; }

    [CommandSwitch("--linked-databases")]
    public string? LinkedDatabases { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CommandSwitch("--module")]
    public string? Module { get; set; }

    [BooleanCommandSwitch("--no-database")]
    public bool? NoDatabase { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--persistence")]
    public string? Persistence { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}

