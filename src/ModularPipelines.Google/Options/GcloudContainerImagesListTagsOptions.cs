using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "images", "list-tags")]
public record GcloudContainerImagesListTagsOptions(
[property: PositionalArgument] string ImageName
) : GcloudOptions;