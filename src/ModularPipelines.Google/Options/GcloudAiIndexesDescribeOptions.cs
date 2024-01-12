using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "indexes", "describe")]
public record GcloudAiIndexesDescribeOptions(
[property: PositionalArgument] string Index,
[property: PositionalArgument] string Region
) : GcloudOptions;