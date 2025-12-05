using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "address-groups", "add-items")]
public record GcloudNetworkSecurityAddressGroupsAddItemsOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--items")]
    public string[]? Items { get; set; }
}