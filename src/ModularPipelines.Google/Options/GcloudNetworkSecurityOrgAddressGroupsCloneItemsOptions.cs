using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "org-address-groups", "clone-items")]
public record GcloudNetworkSecurityOrgAddressGroupsCloneItemsOptions(
[property: PositionalArgument] string AddressGroup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}