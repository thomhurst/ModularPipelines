using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "describe")]
public record GcloudComputeTpusDescribeOptions(
[property: PositionalArgument] string Tpu,
[property: PositionalArgument] string Zone
) : GcloudOptions;