using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "images", "untag")]
public record GcloudContainerImagesUntagOptions(
[property: PositionalArgument] string ImageName
) : GcloudOptions;