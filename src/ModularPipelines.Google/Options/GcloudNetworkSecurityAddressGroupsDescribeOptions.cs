using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "address-groups", "describe")]
public record GcloudNetworkSecurityAddressGroupsDescribeOptions(
[property: PositionalArgument] string AddressGroup,
[property: PositionalArgument] string Location
) : GcloudOptions;