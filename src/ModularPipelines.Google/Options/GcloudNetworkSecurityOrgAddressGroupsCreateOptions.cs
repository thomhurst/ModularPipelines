using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-security", "org-address-groups", "create")]
public record GcloudNetworkSecurityOrgAddressGroupsCreateOptions(
[property: PositionalArgument] string AddressGroup,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization,
[property: CommandSwitch("--capacity")] string Capacity,
[property: CommandSwitch("--type")] string Type
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--items")]
    public string[]? Items { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}