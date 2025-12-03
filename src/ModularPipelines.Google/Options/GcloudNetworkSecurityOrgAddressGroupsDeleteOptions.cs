using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "org-address-groups", "delete")]
public record GcloudNetworkSecurityOrgAddressGroupsDeleteOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location,
[property: CliArgument] string Organization
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}