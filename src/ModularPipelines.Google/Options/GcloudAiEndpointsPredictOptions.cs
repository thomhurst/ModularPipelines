using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "predict")]
public record GcloudAiEndpointsPredictOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Region,
[property: CliOption("--json-request")] string JsonRequest
) : GcloudOptions;