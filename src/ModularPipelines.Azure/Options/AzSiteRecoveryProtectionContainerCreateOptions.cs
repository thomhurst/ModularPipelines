using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "protection-container", "create")]
public record AzSiteRecoveryProtectionContainerCreateOptions(
[property: CliOption("--fabric-name")] string FabricName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--provider-input")]
    public string? ProviderInput { get; set; }
}