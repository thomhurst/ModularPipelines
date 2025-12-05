using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "operations", "list")]
public record GcloudApiGatewayOperationsListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}