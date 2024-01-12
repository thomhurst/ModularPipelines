using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "explain")]
public record GcloudAiEndpointsExplainOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--json-request")] string JsonRequest
) : GcloudOptions
{
    [CommandSwitch("--deployed-model-id")]
    public string? DeployedModelId { get; set; }
}