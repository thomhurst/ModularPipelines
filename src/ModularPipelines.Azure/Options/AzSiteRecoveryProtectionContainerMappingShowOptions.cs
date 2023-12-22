using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "protection-container", "mapping", "show")]
public record AzSiteRecoveryProtectionContainerMappingShowOptions : AzOptions
{
    [CommandSwitch("--fabric-name")]
    public string? FabricName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--mapping-name")]
    public string? MappingName { get; set; }

    [CommandSwitch("--protection-container")]
    public string? ProtectionContainer { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}