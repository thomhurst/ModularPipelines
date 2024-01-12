using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "https-health-checks", "describe")]
public record GcloudComputeHttpsHealthChecksDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;