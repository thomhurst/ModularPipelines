using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "tpu-vm", "describe")]
public record GcloudComputeTpusTpuVmDescribeOptions(
[property: CliArgument] string Tpu,
[property: CliArgument] string Zone
) : GcloudOptions;