using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "describe")]
public record GcloudComputeImagesDescribeOptions(
[property: PositionalArgument] string ImageName
) : GcloudOptions;