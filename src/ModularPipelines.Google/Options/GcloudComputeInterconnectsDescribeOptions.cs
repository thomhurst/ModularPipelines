using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "describe")]
public record GcloudComputeInterconnectsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;