using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "org-address-groups", "describe")]
public record GcloudNetworkSecurityOrgAddressGroupsDescribeOptions(
[property: PositionalArgument] string AddressGroup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization
) : GcloudOptions;