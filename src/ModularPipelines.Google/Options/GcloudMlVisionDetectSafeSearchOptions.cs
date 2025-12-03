using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "detect-safe-search")]
public record GcloudMlVisionDetectSafeSearchOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions;