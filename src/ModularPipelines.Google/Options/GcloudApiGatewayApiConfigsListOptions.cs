using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "api-configs", "list")]
public record GcloudApiGatewayApiConfigsListOptions : GcloudOptions
{
    [CliOption("--api")]
    public string? Api { get; set; }
}