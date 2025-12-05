using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "update")]
public record GcloudComputeRoutersUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--advertisement-mode")]
    public string? AdvertisementMode { get; set; }

    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--keepalive-interval")]
    public string? KeepaliveInterval { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--set-advertisement-groups")]
    public string[]? SetAdvertisementGroups { get; set; }

    [CliOption("--set-advertisement-ranges")]
    public string[]? SetAdvertisementRanges { get; set; }

    [CliOption("--add-advertisement-groups")]
    public string[]? AddAdvertisementGroups { get; set; }

    [CliFlag("ALL_SUBNETS")]
    public bool? AllSubnets { get; set; }

    [CliOption("--add-advertisement-ranges")]
    public string[]? AddAdvertisementRanges { get; set; }

    [CliOption("--remove-advertisement-groups")]
    public string[]? RemoveAdvertisementGroups { get; set; }

    [CliOption("--remove-advertisement-ranges")]
    public string[]? RemoveAdvertisementRanges { get; set; }
}