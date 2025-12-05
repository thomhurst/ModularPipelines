using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "explain")]
public record GcloudAiEndpointsExplainOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Region,
[property: CliOption("--json-request")] string JsonRequest
) : GcloudOptions
{
    [CliOption("--deployed-model-id")]
    public string? DeployedModelId { get; set; }
}