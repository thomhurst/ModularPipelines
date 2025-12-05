using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "packet-mirrorings", "update")]
public record GcloudComputePacketMirroringsUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--collector-ilb")]
    public string? CollectorIlb { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable")]
    public bool? Enable { get; set; }

    [CliOption("--filter-direction")]
    public string? FilterDirection { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--add-filter-cidr-ranges")]
    public string[]? AddFilterCidrRanges { get; set; }

    [CliFlag("--clear-filter-cidr-ranges")]
    public bool? ClearFilterCidrRanges { get; set; }

    [CliOption("--remove-filter-cidr-ranges")]
    public string[]? RemoveFilterCidrRanges { get; set; }

    [CliOption("--set-filter-cidr-ranges")]
    public string[]? SetFilterCidrRanges { get; set; }

    [CliOption("--add-filter-protocols")]
    public string[]? AddFilterProtocols { get; set; }

    [CliFlag("--clear-filter-protocols")]
    public bool? ClearFilterProtocols { get; set; }

    [CliOption("--remove-filter-protocols")]
    public string[]? RemoveFilterProtocols { get; set; }

    [CliOption("--set-filter-protocols")]
    public string[]? SetFilterProtocols { get; set; }

    [CliOption("--add-mirrored-instances")]
    public string[]? AddMirroredInstances { get; set; }

    [CliFlag("--clear-mirrored-instances")]
    public bool? ClearMirroredInstances { get; set; }

    [CliOption("--remove-mirrored-instances")]
    public string[]? RemoveMirroredInstances { get; set; }

    [CliOption("--set-mirrored-instances")]
    public string[]? SetMirroredInstances { get; set; }

    [CliOption("--add-mirrored-subnets")]
    public string[]? AddMirroredSubnets { get; set; }

    [CliFlag("--clear-mirrored-subnets")]
    public bool? ClearMirroredSubnets { get; set; }

    [CliOption("--remove-mirrored-subnets")]
    public string[]? RemoveMirroredSubnets { get; set; }

    [CliOption("--set-mirrored-subnets")]
    public string[]? SetMirroredSubnets { get; set; }

    [CliOption("--add-mirrored-tags")]
    public string[]? AddMirroredTags { get; set; }

    [CliFlag("--clear-mirrored-tags")]
    public bool? ClearMirroredTags { get; set; }

    [CliOption("--remove-mirrored-tags")]
    public string[]? RemoveMirroredTags { get; set; }

    [CliOption("--set-mirrored-tags")]
    public string[]? SetMirroredTags { get; set; }
}