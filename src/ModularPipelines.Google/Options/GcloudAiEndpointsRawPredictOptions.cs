using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "raw-predict")]
public record GcloudAiEndpointsRawPredictOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--request")] string Request
) : GcloudOptions
{
    [CommandSwitch("--http-headers")]
    public string[]? HttpHeaders { get; set; }
}