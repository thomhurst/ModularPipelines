using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("site-recovery", "protected-item", "create")]
public record AzSiteRecoveryProtectedItemCreateOptions(
[property: CliOption("--fabric-name")] string FabricName,
[property: CliOption("--name")] string Name,
[property: CliOption("--protection-container")] string ProtectionContainer,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--policy-id")]
    public string? PolicyId { get; set; }

    [CliOption("--protectable-item-id")]
    public string? ProtectableItemId { get; set; }

    [CliOption("--provider-details")]
    public string? ProviderDetails { get; set; }
}