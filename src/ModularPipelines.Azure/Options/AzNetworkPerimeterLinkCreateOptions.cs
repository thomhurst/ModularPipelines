using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "perimeter", "link", "create")]
public record AzNetworkPerimeterLinkCreateOptions(
[property: CliOption("--link-name")] string LinkName,
[property: CliOption("--perimeter-name")] string PerimeterName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--auto-remote-nsp-id")]
    public string? AutoRemoteNspId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--local-inbound-profile")]
    public string? LocalInboundProfile { get; set; }

    [CliOption("--remote-inbound-profile")]
    public string? RemoteInboundProfile { get; set; }
}