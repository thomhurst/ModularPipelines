using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("redisenterprise", "update")]
public record AzRedisenterpriseUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--assigned-identities")]
    public string? AssignedIdentities { get; set; }

    [CliOption("--capacity")]
    public string? Capacity { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity-resource-id")]
    public string? IdentityResourceId { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-encryption-identity-type")]
    public string? KeyEncryptionIdentityType { get; set; }

    [CliOption("--key-encryption-key-url")]
    public string? KeyEncryptionKeyUrl { get; set; }

    [CliOption("--minimum-tls-version")]
    public string? MinimumTlsVersion { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}