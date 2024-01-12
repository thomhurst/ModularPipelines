using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routers", "create")]
public record GcloudComputeRoutersCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions
{
    [CommandSwitch("--advertisement-mode")]
    public string? AdvertisementMode { get; set; }

    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--encrypted-interconnect-router")]
    public bool? EncryptedInterconnectRouter { get; set; }

    [CommandSwitch("--keepalive-interval")]
    public string? KeepaliveInterval { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--set-advertisement-groups")]
    public string[]? SetAdvertisementGroups { get; set; }

    [CommandSwitch("--set-advertisement-ranges")]
    public string[]? SetAdvertisementRanges { get; set; }
}