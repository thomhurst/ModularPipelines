using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "images", "untag")]
public record GcloudContainerImagesUntagOptions(
[property: CliArgument] string ImageName
) : GcloudOptions;