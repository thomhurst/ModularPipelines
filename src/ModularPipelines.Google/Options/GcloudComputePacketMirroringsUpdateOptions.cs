using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "packet-mirrorings", "update")]
public record GcloudComputePacketMirroringsUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--collector-ilb")]
    public string? CollectorIlb { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable")]
    public bool? Enable { get; set; }

    [CommandSwitch("--filter-direction")]
    public string? FilterDirection { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--add-filter-cidr-ranges")]
    public string[]? AddFilterCidrRanges { get; set; }

    [BooleanCommandSwitch("--clear-filter-cidr-ranges")]
    public bool? ClearFilterCidrRanges { get; set; }

    [CommandSwitch("--remove-filter-cidr-ranges")]
    public string[]? RemoveFilterCidrRanges { get; set; }

    [CommandSwitch("--set-filter-cidr-ranges")]
    public string[]? SetFilterCidrRanges { get; set; }

    [CommandSwitch("--add-filter-protocols")]
    public string[]? AddFilterProtocols { get; set; }

    [BooleanCommandSwitch("--clear-filter-protocols")]
    public bool? ClearFilterProtocols { get; set; }

    [CommandSwitch("--remove-filter-protocols")]
    public string[]? RemoveFilterProtocols { get; set; }

    [CommandSwitch("--set-filter-protocols")]
    public string[]? SetFilterProtocols { get; set; }

    [CommandSwitch("--add-mirrored-instances")]
    public string[]? AddMirroredInstances { get; set; }

    [BooleanCommandSwitch("--clear-mirrored-instances")]
    public bool? ClearMirroredInstances { get; set; }

    [CommandSwitch("--remove-mirrored-instances")]
    public string[]? RemoveMirroredInstances { get; set; }

    [CommandSwitch("--set-mirrored-instances")]
    public string[]? SetMirroredInstances { get; set; }

    [CommandSwitch("--add-mirrored-subnets")]
    public string[]? AddMirroredSubnets { get; set; }

    [BooleanCommandSwitch("--clear-mirrored-subnets")]
    public bool? ClearMirroredSubnets { get; set; }

    [CommandSwitch("--remove-mirrored-subnets")]
    public string[]? RemoveMirroredSubnets { get; set; }

    [CommandSwitch("--set-mirrored-subnets")]
    public string[]? SetMirroredSubnets { get; set; }

    [CommandSwitch("--add-mirrored-tags")]
    public string[]? AddMirroredTags { get; set; }

    [BooleanCommandSwitch("--clear-mirrored-tags")]
    public bool? ClearMirroredTags { get; set; }

    [CommandSwitch("--remove-mirrored-tags")]
    public string[]? RemoveMirroredTags { get; set; }

    [CommandSwitch("--set-mirrored-tags")]
    public string[]? SetMirroredTags { get; set; }
}