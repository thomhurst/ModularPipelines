using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "protected-item", "create")]
public record AzSiteRecoveryProtectedItemCreateOptions(
[property: CommandSwitch("--fabric-name")] string FabricName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--protection-container")] string ProtectionContainer,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--policy-id")]
    public string? PolicyId { get; set; }

    [CommandSwitch("--protectable-item-id")]
    public string? ProtectableItemId { get; set; }

    [CommandSwitch("--provider-details")]
    public string? ProviderDetails { get; set; }
}