using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "describe")]
public record GcloudComputeImagesDescribeOptions(
[property: CliArgument] string ImageName
) : GcloudOptions;