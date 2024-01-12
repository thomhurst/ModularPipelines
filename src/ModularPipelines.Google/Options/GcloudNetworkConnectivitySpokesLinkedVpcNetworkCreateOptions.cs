using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "spokes", "linked-vpc-network", "create")]
public record GcloudNetworkConnectivitySpokesLinkedVpcNetworkCreateOptions(
[property: PositionalArgument] string Spoke,
[property: CommandSwitch("--hub")] string Hub,
[property: CommandSwitch("--vpc-network")] string VpcNetwork
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--exclude-export-ranges")]
    public string[]? ExcludeExportRanges { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}