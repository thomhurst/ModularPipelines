using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-vpn-gateways", "list")]
public record GcloudComputeTargetVpnGatewaysListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }
}