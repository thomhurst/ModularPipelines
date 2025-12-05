using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "address-groups", "list-references")]
public record GcloudNetworkSecurityAddressGroupsListReferencesOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location
) : GcloudOptions;