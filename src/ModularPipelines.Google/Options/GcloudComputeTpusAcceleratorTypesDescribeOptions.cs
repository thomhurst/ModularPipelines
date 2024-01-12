using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "accelerator-types", "describe")]
public record GcloudComputeTpusAcceleratorTypesDescribeOptions(
[property: PositionalArgument] string AcceleratorType,
[property: PositionalArgument] string Zone
) : GcloudOptions;