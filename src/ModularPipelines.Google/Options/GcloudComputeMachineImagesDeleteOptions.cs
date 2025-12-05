using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "machine-images", "delete")]
public record GcloudComputeMachineImagesDeleteOptions(
[property: CliArgument] string Image
) : GcloudOptions;