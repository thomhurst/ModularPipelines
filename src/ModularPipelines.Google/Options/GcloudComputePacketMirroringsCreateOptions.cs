using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "packet-mirrorings", "create")]
public record GcloudComputePacketMirroringsCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--collector-ilb")] string CollectorIlb,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable")]
    public bool? Enable { get; set; }

    [CommandSwitch("--filter-cidr-ranges")]
    public string[]? FilterCidrRanges { get; set; }

    [CommandSwitch("--filter-direction")]
    public string? FilterDirection { get; set; }

    [CommandSwitch("--filter-protocols")]
    public string[]? FilterProtocols { get; set; }

    [CommandSwitch("--mirrored-instances")]
    public string[]? MirroredInstances { get; set; }

    [CommandSwitch("--mirrored-subnets")]
    public string[]? MirroredSubnets { get; set; }

    [CommandSwitch("--mirrored-tags")]
    public string[]? MirroredTags { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}