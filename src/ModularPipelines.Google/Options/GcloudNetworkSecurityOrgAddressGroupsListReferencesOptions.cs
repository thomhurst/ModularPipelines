using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "org-address-groups", "list-references")]
public record GcloudNetworkSecurityOrgAddressGroupsListReferencesOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location,
[property: CliArgument] string Organization
) : GcloudOptions;