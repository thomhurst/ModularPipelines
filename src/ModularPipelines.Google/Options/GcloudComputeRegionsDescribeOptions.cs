using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "regions", "describe")]
public record GcloudComputeRegionsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;