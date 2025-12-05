using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "delete")]
public record GcloudComputeImagesDeleteOptions(
[property: CliArgument] string ImageName
) : GcloudOptions;