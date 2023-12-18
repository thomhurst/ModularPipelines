using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "protected-item", "planned-failover")]
public record AzSiteRecoveryProtectedItemPlannedFailoverOptions : AzOptions
{
    [CommandSwitch("--fabric-name")]
    public string? FabricName { get; set; }

    [CommandSwitch("--failover-direction")]
    public string? FailoverDirection { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protection-container")]
    public string? ProtectionContainer { get; set; }

    [CommandSwitch("--provider-details")]
    public string? ProviderDetails { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}