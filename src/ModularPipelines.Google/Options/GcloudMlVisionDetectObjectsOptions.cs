using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "vision", "detect-objects")]
public record GcloudMlVisionDetectObjectsOptions(
[property: PositionalArgument] string ImagePath
) : GcloudOptions;