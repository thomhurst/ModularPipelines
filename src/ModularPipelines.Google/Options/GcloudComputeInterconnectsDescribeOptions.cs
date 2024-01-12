using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "describe")]
public record GcloudComputeInterconnectsDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;