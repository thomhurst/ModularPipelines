using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "machine-images", "describe")]
public record GcloudComputeMachineImagesDescribeOptions(
[property: PositionalArgument] string Image
) : GcloudOptions;