using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-security", "address-groups", "create")]
public record GcloudNetworkSecurityAddressGroupsCreateOptions(
[property: CliArgument] string AddressGroup,
[property: CliArgument] string Location,
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