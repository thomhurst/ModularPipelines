using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-analytics", "data-product", "create")]
public record AzNetworkAnalyticsDataProductCreateOptions(
[property: CommandSwitch("--data-product-name")] string DataProductName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [BooleanCommandSwitch("--key-encryption-enable")]
    public bool? KeyEncryptionEnable { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--major-version")]
    public string? MajorVersion { get; set; }

    [CommandSwitch("--managed-rg")]
    public string? ManagedRg { get; set; }

    [CommandSwitch("--networkacls")]
    public string? Networkacls { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--owners")]
    public string? Owners { get; set; }

    [BooleanCommandSwitch("--private-links-enabled")]
    public bool? PrivateLinksEnabled { get; set; }

    [CommandSwitch("--product")]
    public string? Product { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--publisher")]
    public string? Publisher { get; set; }

    [CommandSwitch("--purview-account")]
    public int? PurviewAccount { get; set; }

    [CommandSwitch("--purview-collection")]
    public string? PurviewCollection { get; set; }

    [CommandSwitch("--redundancy")]
    public string? Redundancy { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}