using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "undeploy-model")]
public record GcloudAiEndpointsUndeployModelOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Region,
[property: CliOption("--deployed-model-id")] string DeployedModelId
) : GcloudOptions
{
    [CliOption("--traffic-split")]
    public string[]? TrafficSplit { get; set; }
}