using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "address-groups", "clone-items")]
public record GcloudNetworkSecurityAddressGroupsCloneItemsOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }
}