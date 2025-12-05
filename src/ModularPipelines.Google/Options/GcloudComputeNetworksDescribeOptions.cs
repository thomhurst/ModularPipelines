using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "networks", "describe")]
public record GcloudComputeNetworksDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;