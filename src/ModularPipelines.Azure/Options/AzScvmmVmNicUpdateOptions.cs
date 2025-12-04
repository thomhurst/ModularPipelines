using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("scvmm", "vm", "nic", "update")]
public record AzScvmmVmNicUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliFlag("--disconnect")]
    public bool? Disconnect { get; set; }

    [CliOption("--ipv4-address-type")]
    public string? Ipv4AddressType { get; set; }

    [CliOption("--ipv6-address-type")]
    public string? Ipv6AddressType { get; set; }

    [CliOption("--mac-address-type")]
    public string? MacAddressType { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--nic-id")]
    public string? NicId { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}