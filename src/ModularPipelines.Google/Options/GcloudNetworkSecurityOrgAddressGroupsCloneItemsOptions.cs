using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "org-address-groups", "clone-items")]
public record GcloudNetworkSecurityOrgAddressGroupsCloneItemsOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location,
[property: CliArgument] string Organization
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}