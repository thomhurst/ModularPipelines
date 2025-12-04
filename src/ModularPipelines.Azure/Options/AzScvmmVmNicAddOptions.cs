using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("scvmm", "vm", "nic", "add")]
public record AzScvmmVmNicAddOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--ipv4-address-type")]
    public string? Ipv4AddressType { get; set; }

    [CliOption("--ipv6-address-type")]
    public string? Ipv6AddressType { get; set; }

    [CliOption("--mac-address")]
    public string? MacAddress { get; set; }

    [CliOption("--mac-address-type")]
    public string? MacAddressType { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}