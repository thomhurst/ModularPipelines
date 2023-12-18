using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vm", "nic", "list")]
public record AzScvmmVmNicListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [BooleanCommandSwitch("--disconnect")]
    public bool? Disconnect { get; set; }

    [CommandSwitch("--ipv4-address-type")]
    public string? Ipv4AddressType { get; set; }

    [CommandSwitch("--ipv6-address-type")]
    public string? Ipv6AddressType { get; set; }

    [CommandSwitch("--mac-address-type")]
    public string? MacAddressType { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--nic-id")]
    public string? NicId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}