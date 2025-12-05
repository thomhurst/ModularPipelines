using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "regions", "describe")]
public record GcloudComputeRegionsDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;