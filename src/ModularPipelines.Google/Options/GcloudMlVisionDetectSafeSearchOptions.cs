using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "vision", "detect-safe-search")]
public record GcloudMlVisionDetectSafeSearchOptions(
[property: PositionalArgument] string ImagePath
) : GcloudOptions;