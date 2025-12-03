using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "network-peerings", "list")]
public record GcloudVmwareNetworkPeeringsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}