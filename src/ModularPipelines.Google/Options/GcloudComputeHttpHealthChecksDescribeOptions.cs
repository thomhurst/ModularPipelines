using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "http-health-checks", "describe")]
public record GcloudComputeHttpHealthChecksDescribeOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;