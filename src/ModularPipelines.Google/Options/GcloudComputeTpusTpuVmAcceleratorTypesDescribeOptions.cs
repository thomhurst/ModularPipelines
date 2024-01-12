using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "tpu-vm", "accelerator-types", "describe")]
public record GcloudComputeTpusTpuVmAcceleratorTypesDescribeOptions(
[property: PositionalArgument] string AcceleratorType,
[property: PositionalArgument] string Zone
) : GcloudOptions;