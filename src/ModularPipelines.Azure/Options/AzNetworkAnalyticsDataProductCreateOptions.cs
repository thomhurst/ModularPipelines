using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network-analytics", "data-product", "create")]
public record AzNetworkAnalyticsDataProductCreateOptions(
[property: CliOption("--data-product-name")] string DataProductName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliFlag("--key-encryption-enable")]
    public bool? KeyEncryptionEnable { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--major-version")]
    public string? MajorVersion { get; set; }

    [CliOption("--managed-rg")]
    public string? ManagedRg { get; set; }

    [CliOption("--networkacls")]
    public string? Networkacls { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--owners")]
    public string? Owners { get; set; }

    [CliFlag("--private-links-enabled")]
    public bool? PrivateLinksEnabled { get; set; }

    [CliOption("--product")]
    public string? Product { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--purview-account")]
    public int? PurviewAccount { get; set; }

    [CliOption("--purview-collection")]
    public string? PurviewCollection { get; set; }

    [CliOption("--redundancy")]
    public string? Redundancy { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}