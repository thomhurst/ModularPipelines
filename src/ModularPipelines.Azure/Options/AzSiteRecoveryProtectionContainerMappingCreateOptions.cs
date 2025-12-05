using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "protection-container", "mapping", "create")]
public record AzSiteRecoveryProtectionContainerMappingCreateOptions(
[property: CliOption("--fabric-name")] string FabricName,
[property: CliOption("--mapping-name")] string MappingName,
[property: CliOption("--protection-container")] string ProtectionContainer,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--policy-id")]
    public string? PolicyId { get; set; }

    [CliOption("--provider-input")]
    public string? ProviderInput { get; set; }

    [CliOption("--target-container")]
    public string? TargetContainer { get; set; }
}