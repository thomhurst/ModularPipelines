using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "tpus", "describe")]
public record GcloudComputeTpusDescribeOptions(
[property: CliArgument] string Tpu,
[property: CliArgument] string Zone
) : GcloudOptions;