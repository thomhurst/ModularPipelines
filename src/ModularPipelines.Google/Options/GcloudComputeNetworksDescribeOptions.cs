using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "describe")]
public record GcloudComputeNetworksDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;