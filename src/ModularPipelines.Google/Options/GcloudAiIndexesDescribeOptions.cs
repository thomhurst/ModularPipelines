using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "indexes", "describe")]
public record GcloudAiIndexesDescribeOptions(
[property: CliArgument] string Index,
[property: CliArgument] string Region
) : GcloudOptions;