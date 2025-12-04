using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "protection-container", "mapping", "update")]
public record AzSiteRecoveryProtectionContainerMappingUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--fabric-name")]
    public string? FabricName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--mapping-name")]
    public string? MappingName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--policy-id")]
    public string? PolicyId { get; set; }

    [CliOption("--protection-container")]
    public string? ProtectionContainer { get; set; }

    [CliOption("--provider-input")]
    public string? ProviderInput { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-container")]
    public string? TargetContainer { get; set; }

    [CliOption("--vault-name")]
    public string? VaultName { get; set; }
}