using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "address-groups", "add-items")]
public record GcloudNetworkSecurityAddressGroupsAddItemsOptions(
[property: PositionalArgument] string AddressGroup,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--items")]
    public string[]? Items { get; set; }
}