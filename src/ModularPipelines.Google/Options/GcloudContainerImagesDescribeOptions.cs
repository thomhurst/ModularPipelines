using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "images", "describe")]
public record GcloudContainerImagesDescribeOptions(
[property: PositionalArgument] string ImageName
) : GcloudOptions;