using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "org-address-groups", "create")]
public record GcloudNetworkSecurityOrgAddressGroupsCreateOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location,
[property: CliArgument] string Organization,
[property: CliOption("--capacity")] string Capacity,
[property: CliOption("--type")] string Type
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--items")]
    public string[]? Items { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}