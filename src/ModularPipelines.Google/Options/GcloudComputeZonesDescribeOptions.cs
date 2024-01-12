using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "zones", "describe")]
public record GcloudComputeZonesDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;