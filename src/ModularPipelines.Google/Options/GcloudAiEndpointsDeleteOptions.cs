using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "delete")]
public record GcloudAiEndpointsDeleteOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Region
) : GcloudOptions;