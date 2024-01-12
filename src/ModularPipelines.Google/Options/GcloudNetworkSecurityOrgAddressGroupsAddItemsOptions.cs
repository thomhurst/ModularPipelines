using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "org-address-groups", "add-items")]
public record GcloudNetworkSecurityOrgAddressGroupsAddItemsOptions(
[property: PositionalArgument] string AddressGroup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--items")]
    public string[]? Items { get; set; }
}