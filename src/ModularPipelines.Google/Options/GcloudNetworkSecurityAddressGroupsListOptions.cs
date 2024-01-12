using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "address-groups", "list")]
public record GcloudNetworkSecurityAddressGroupsListOptions(
[property: CommandSwitch("--location")] string Location
) : GcloudOptions;