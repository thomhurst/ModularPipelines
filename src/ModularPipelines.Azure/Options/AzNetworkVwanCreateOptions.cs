using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vwan", "create")]
public record AzNetworkVwanCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--branch-to-branch-traffic")]
    public bool? BranchToBranchTraffic { get; set; }

    [BooleanCommandSwitch("--disable-vpn-encryption")]
    public bool? DisableVpnEncryption { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--office365-category")]
    public string? Office365Category { get; set; }

    [CommandSwitch("--security-provider-name")]
    public string? SecurityProviderName { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}