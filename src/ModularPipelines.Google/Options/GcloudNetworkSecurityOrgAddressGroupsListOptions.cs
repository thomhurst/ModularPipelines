using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "org-address-groups", "list")]
public record GcloudNetworkSecurityOrgAddressGroupsListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--organization")] string Organization
) : GcloudOptions;