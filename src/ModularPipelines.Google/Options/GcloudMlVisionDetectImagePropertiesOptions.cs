using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "vision", "detect-image-properties")]
public record GcloudMlVisionDetectImagePropertiesOptions(
[property: PositionalArgument] string ImagePath
) : GcloudOptions;