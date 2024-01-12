using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "predict")]
public record GcloudAiEndpointsPredictOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--json-request")] string JsonRequest
) : GcloudOptions;