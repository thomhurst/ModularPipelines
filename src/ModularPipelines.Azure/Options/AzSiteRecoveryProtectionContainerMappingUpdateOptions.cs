using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("site-recovery", "protection-container", "mapping", "update")]
public record AzSiteRecoveryProtectionContainerMappingUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--fabric-name")]
    public string? FabricName { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--mapping-name")]
    public string? MappingName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--policy-id")]
    public string? PolicyId { get; set; }

    [CommandSwitch("--protection-container")]
    public string? ProtectionContainer { get; set; }

    [CommandSwitch("--provider-input")]
    public string? ProviderInput { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--target-container")]
    public string? TargetContainer { get; set; }

    [CommandSwitch("--vault-name")]
    public string? VaultName { get; set; }
}