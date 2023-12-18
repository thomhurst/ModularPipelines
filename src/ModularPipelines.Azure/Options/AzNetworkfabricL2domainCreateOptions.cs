using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "l2domain", "create")]
public record AzNetworkfabricL2domainCreateOptions(
[property: CommandSwitch("--nf-id")] string NfId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--vlan-id")] string VlanId
) : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}