using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "describe")]
public record GcloudComputeMachineImagesDescribeOptions(
[property: CliArgument] string Image
) : GcloudOptions;