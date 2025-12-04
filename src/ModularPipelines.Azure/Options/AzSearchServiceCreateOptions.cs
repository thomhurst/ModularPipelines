using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("search", "service", "create")]
public record AzSearchServiceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--aad-auth-failure-mode")]
    public string? AadAuthFailureMode { get; set; }

    [CliOption("--auth-options")]
    public string? AuthOptions { get; set; }

    [CliFlag("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [CliOption("--hosting-mode")]
    public string? HostingMode { get; set; }

    [CliOption("--identity-type")]
    public string? IdentityType { get; set; }

    [CliOption("--ip-rules")]
    public string? IpRules { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--partition-count")]
    public int? PartitionCount { get; set; }

    [CliOption("--public-access")]
    public string? PublicAccess { get; set; }

    [CliOption("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CliOption("--semantic-search")]
    public string? SemanticSearch { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}