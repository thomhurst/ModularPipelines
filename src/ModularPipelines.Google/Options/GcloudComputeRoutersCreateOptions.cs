using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "create")]
public record GcloudComputeRoutersCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliOption("--advertisement-mode")]
    public string? AdvertisementMode { get; set; }

    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--encrypted-interconnect-router")]
    public bool? EncryptedInterconnectRouter { get; set; }

    [CliOption("--keepalive-interval")]
    public string? KeepaliveInterval { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--set-advertisement-groups")]
    public string[]? SetAdvertisementGroups { get; set; }

    [CliOption("--set-advertisement-ranges")]
    public string[]? SetAdvertisementRanges { get; set; }
}