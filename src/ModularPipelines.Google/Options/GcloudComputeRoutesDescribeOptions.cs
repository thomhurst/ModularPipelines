using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "routes", "describe")]
public record GcloudComputeRoutesDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;