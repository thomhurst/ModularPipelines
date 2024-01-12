using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "internal-ranges", "update")]
public record GcloudNetworkConnectivityInternalRangesUpdateOptions(
[property: PositionalArgument] string InternalRange,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--clear-overlaps")]
    public bool? ClearOverlaps { get; set; }

    [CommandSwitch("--overlaps")]
    public string[]? Overlaps { get; set; }

    [BooleanCommandSwitch("overlap-existing-subnet-range")]
    public bool? OverlapExistingSubnetRange { get; set; }

    [BooleanCommandSwitch("overlap-route-range")]
    public bool? OverlapRouteRange { get; set; }

    [CommandSwitch("--ip-cidr-range")]
    public string? IpCidrRange { get; set; }

    [CommandSwitch("--prefix-length")]
    public string? PrefixLength { get; set; }
}