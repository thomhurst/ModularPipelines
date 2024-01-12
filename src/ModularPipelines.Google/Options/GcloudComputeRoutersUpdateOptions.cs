using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "update")]
public record GcloudComputeRoutersUpdateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--advertisement-mode")]
    public string? AdvertisementMode { get; set; }

    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--keepalive-interval")]
    public string? KeepaliveInterval { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--set-advertisement-groups")]
    public string[]? SetAdvertisementGroups { get; set; }

    [CommandSwitch("--set-advertisement-ranges")]
    public string[]? SetAdvertisementRanges { get; set; }

    [CommandSwitch("--add-advertisement-groups")]
    public string[]? AddAdvertisementGroups { get; set; }

    [BooleanCommandSwitch("ALL_SUBNETS")]
    public bool? AllSubnets { get; set; }

    [CommandSwitch("--add-advertisement-ranges")]
    public string[]? AddAdvertisementRanges { get; set; }

    [CommandSwitch("--remove-advertisement-groups")]
    public string[]? RemoveAdvertisementGroups { get; set; }

    [CommandSwitch("--remove-advertisement-ranges")]
    public string[]? RemoveAdvertisementRanges { get; set; }
}