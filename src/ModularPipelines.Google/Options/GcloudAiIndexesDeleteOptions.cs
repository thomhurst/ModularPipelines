using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "indexes", "delete")]
public record GcloudAiIndexesDeleteOptions(
[property: PositionalArgument] string Index,
[property: PositionalArgument] string Region
) : GcloudOptions;