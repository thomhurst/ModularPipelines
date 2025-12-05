using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vwan", "create")]
public record AzNetworkVwanCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--branch-to-branch-traffic")]
    public bool? BranchToBranchTraffic { get; set; }

    [CliFlag("--disable-vpn-encryption")]
    public bool? DisableVpnEncryption { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--office365-category")]
    public string? Office365Category { get; set; }

    [CliOption("--security-provider-name")]
    public string? SecurityProviderName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}