using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "images", "list-tags")]
public record GcloudContainerImagesListTagsOptions(
[property: CliArgument] string ImageName
) : GcloudOptions;