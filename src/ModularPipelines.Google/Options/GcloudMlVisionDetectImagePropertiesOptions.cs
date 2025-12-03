using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "detect-image-properties")]
public record GcloudMlVisionDetectImagePropertiesOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions;