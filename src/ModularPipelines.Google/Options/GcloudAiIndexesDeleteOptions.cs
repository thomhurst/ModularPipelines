using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "indexes", "delete")]
public record GcloudAiIndexesDeleteOptions(
[property: CliArgument] string Index,
[property: CliArgument] string Region
) : GcloudOptions;