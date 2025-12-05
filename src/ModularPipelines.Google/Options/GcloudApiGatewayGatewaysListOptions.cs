using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "gateways", "list")]
public record GcloudApiGatewayGatewaysListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}