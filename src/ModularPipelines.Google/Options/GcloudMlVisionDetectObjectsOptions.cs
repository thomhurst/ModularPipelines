using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ml", "vision", "detect-objects")]
public record GcloudMlVisionDetectObjectsOptions(
[property: CliArgument] string ImagePath
) : GcloudOptions;