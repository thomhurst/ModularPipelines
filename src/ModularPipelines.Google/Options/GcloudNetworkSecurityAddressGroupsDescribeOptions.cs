using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "address-groups", "describe")]
public record GcloudNetworkSecurityAddressGroupsDescribeOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location
) : GcloudOptions;