using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "protection-container", "switch-protection")]
public record AzSiteRecoveryProtectionContainerSwitchProtectionOptions : AzOptions
{
    [CommandSwitch("--fabric-name")]
    public string? FabricName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protected-item")]
    public string? ProtectedItem { get; set; }

    [CommandSwitch("--protection-container-name")]
    public string? ProtectionContainerName { get; set; }

    [CommandSwitch("--provider-details")]
    public string? ProviderDetails { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}