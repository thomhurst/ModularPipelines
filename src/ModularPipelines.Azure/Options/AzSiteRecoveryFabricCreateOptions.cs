using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("site-recovery", "fabric", "create")]
public record AzSiteRecoveryFabricCreateOptions(
[property: CliOption("--fabric-name")] string FabricName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vault-name")] string VaultName
) : AzOptions
{
    [CliOption("--custom-details")]
    public string? CustomDetails { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}