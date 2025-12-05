using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "raw-predict")]
public record GcloudAiEndpointsRawPredictOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Region,
[property: CliOption("--request")] string Request
) : GcloudOptions
{
    [CliOption("--http-headers")]
    public string[]? HttpHeaders { get; set; }
}