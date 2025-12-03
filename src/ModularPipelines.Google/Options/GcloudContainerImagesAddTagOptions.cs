using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "images", "add-tag")]
public record GcloudContainerImagesAddTagOptions(
[property: CliArgument] string SrcImage,
[property: CliArgument] string DestImage
) : GcloudOptions;