using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "images", "describe")]
public record GcloudContainerImagesDescribeOptions(
[property: CliArgument] string ImageName
) : GcloudOptions;