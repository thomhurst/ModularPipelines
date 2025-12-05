using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "vpn-gateways", "create")]
public record GcloudComputeVpnGatewaysCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--network")] string Network
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--interconnect-attachments")]
    public string[]? InterconnectAttachments { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }
}