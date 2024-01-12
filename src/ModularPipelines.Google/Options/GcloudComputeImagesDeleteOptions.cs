using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "delete")]
public record GcloudComputeImagesDeleteOptions(
[property: PositionalArgument] string ImageName
) : GcloudOptions;