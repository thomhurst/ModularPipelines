using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm", "vm", "nic", "add")]
public record AzScvmmVmNicAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--ipv4-address-type")]
    public string? Ipv4AddressType { get; set; }

    [CommandSwitch("--ipv6-address-type")]
    public string? Ipv6AddressType { get; set; }

    [CommandSwitch("--mac-address")]
    public string? MacAddress { get; set; }

    [CommandSwitch("--mac-address-type")]
    public string? MacAddressType { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}