using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "recovery-plan", "create")]
public record AzSiteRecoveryRecoveryPlanCreateOptions(
[property: CommandSwitch("--groups")] string Groups,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--primary-fabric-id")] string PrimaryFabricId,
[property: CommandSwitch("--recovery-fabric-id")] string RecoveryFabricId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--failover-deploy-model")]
    public string? FailoverDeployModel { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--provider-input")]
    public string? ProviderInput { get; set; }
}