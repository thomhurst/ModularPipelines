using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "vpn-connections", "list")]
public record GcloudEdgeCloudContainerVpnConnectionsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}