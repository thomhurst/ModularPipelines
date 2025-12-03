using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "accelerator-types", "describe")]
public record GcloudComputeTpusAcceleratorTypesDescribeOptions(
[property: CliArgument] string AcceleratorType,
[property: CliArgument] string Zone
) : GcloudOptions;