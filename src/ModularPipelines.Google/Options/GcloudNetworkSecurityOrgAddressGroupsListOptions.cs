using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "org-address-groups", "list")]
public record GcloudNetworkSecurityOrgAddressGroupsListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--organization")] string Organization
) : GcloudOptions;