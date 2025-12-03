using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("site-recovery", "recovery-plan", "create")]
public record AzSiteRecoveryRecoveryPlanCreateOptions(
[property: CliOption("--groups")] string Groups,
[property: CliOption("--name")] string Name,
[property: CliOption("--primary-fabric-id")] string PrimaryFabricId,
[property: CliOption("--recovery-fabric-id")] string RecoveryFabricId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--failover-deploy-model")]
    public string? FailoverDeployModel { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--provider-input")]
    public string? ProviderInput { get; set; }
}