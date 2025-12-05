using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "accelerator-types", "describe")]
public record GcloudComputeTpusTpuVmAcceleratorTypesDescribeOptions(
[property: CliArgument] string AcceleratorType,
[property: CliArgument] string Zone
) : GcloudOptions;