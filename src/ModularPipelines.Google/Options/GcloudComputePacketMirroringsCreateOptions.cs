using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "packet-mirrorings", "create")]
public record GcloudComputePacketMirroringsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--collector-ilb")] string CollectorIlb,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable")]
    public bool? Enable { get; set; }

    [CliOption("--filter-cidr-ranges")]
    public string[]? FilterCidrRanges { get; set; }

    [CliOption("--filter-direction")]
    public string? FilterDirection { get; set; }

    [CliOption("--filter-protocols")]
    public string[]? FilterProtocols { get; set; }

    [CliOption("--mirrored-instances")]
    public string[]? MirroredInstances { get; set; }

    [CliOption("--mirrored-subnets")]
    public string[]? MirroredSubnets { get; set; }

    [CliOption("--mirrored-tags")]
    public string[]? MirroredTags { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}