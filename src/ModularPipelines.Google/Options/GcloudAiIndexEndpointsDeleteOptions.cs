using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "index-endpoints", "delete")]
public record GcloudAiIndexEndpointsDeleteOptions(
[property: CliArgument] string IndexEndpoint,
[property: CliArgument] string Region
) : GcloudOptions;