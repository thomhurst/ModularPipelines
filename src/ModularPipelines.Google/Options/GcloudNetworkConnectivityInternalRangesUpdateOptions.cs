using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "internal-ranges", "update")]
public record GcloudNetworkConnectivityInternalRangesUpdateOptions(
[property: CliArgument] string InternalRange,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--clear-overlaps")]
    public bool? ClearOverlaps { get; set; }

    [CliOption("--overlaps")]
    public string[]? Overlaps { get; set; }

    [CliFlag("overlap-existing-subnet-range")]
    public bool? OverlapExistingSubnetRange { get; set; }

    [CliFlag("overlap-route-range")]
    public bool? OverlapRouteRange { get; set; }

    [CliOption("--ip-cidr-range")]
    public string? IpCidrRange { get; set; }

    [CliOption("--prefix-length")]
    public string? PrefixLength { get; set; }
}