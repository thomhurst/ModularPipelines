using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "external-vpn-gateways", "create")]
public record GcloudComputeExternalVpnGatewaysCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--interfaces")] string[] Interfaces
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}