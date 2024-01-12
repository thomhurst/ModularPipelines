using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "machine-images", "delete")]
public record GcloudComputeMachineImagesDeleteOptions(
[property: PositionalArgument] string Image
) : GcloudOptions;