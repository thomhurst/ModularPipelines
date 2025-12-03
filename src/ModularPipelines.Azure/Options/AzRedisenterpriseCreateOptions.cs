using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redisenterprise", "create")]
public record AzRedisenterpriseCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--assigned-identities")]
    public string? AssignedIdentities { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--client-protocol")]
    public string? ClientProtocol { get; set; }

    [CliOption("--clustering-policy")]
    public string? ClusteringPolicy { get; set; }

    [CliOption("--eviction-policy")]
    public string? EvictionPolicy { get; set; }

    [CliOption("--group-nickname")]
    public string? GroupNickname { get; set; }

    [CliOption("--identity-resource-id")]
    public string? IdentityResourceId { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--key-encryption-identity-type")]
    public string? KeyEncryptionIdentityType { get; set; }

    [CliOption("--key-encryption-key-url")]
    public string? KeyEncryptionKeyUrl { get; set; }

    [CliOption("--linked-databases")]
    public string? LinkedDatabases { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CliOption("--module")]
    public string? Module { get; set; }

    [CliFlag("--no-database")]
    public bool? NoDatabase { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--persistence")]
    public string? Persistence { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}