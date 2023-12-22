using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "protection-container", "mapping", "create")]
public record AzSiteRecoveryProtectionContainerMappingCreateOptions(
[property: CommandSwitch("--fabric-name")] string FabricName,
[property: CommandSwitch("--mapping-name")] string MappingName,
[property: CommandSwitch("--protection-container")] string ProtectionContainer,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--policy-id")]
    public string? PolicyId { get; set; }

    [CommandSwitch("--provider-input")]
    public string? ProviderInput { get; set; }

    [CommandSwitch("--target-container")]
    public string? TargetContainer { get; set; }
}