using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "protectable-item", "list")]
public record AzSiteRecoveryProtectableItemListOptions(
[property: CommandSwitch("--fabric-name")] string FabricName,
[property: CommandSwitch("--protection-container")] string ProtectionContainer,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vault-name")] string VaultName
) : AzOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }

    [CommandSwitch("--take")]
    public string? Take { get; set; }
}