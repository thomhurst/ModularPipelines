using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "address-groups", "list-references")]
public record GcloudNetworkSecurityAddressGroupsListReferencesOptions(
[property: PositionalArgument] string AddressGroup,
[property: PositionalArgument] string Location
) : GcloudOptions;