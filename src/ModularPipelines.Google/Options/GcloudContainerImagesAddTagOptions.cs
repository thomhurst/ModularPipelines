using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "images", "add-tag")]
public record GcloudContainerImagesAddTagOptions(
[property: PositionalArgument] string SrcImage,
[property: PositionalArgument] string DestImage
) : GcloudOptions;