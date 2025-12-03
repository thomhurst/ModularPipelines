using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "address-groups", "list")]
public record GcloudNetworkSecurityAddressGroupsListOptions(
[property: CliOption("--location")] string Location
) : GcloudOptions;