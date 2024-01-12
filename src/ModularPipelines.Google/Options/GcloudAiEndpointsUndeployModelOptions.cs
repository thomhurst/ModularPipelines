using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "undeploy-model")]
public record GcloudAiEndpointsUndeployModelOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--deployed-model-id")] string DeployedModelId
) : GcloudOptions
{
    [CommandSwitch("--traffic-split")]
    public string[]? TrafficSplit { get; set; }
}