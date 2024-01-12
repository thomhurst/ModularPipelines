using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("endpoints", "operations", "describe")]
public record GcloudEndpointsOperationsDescribeOptions(
[property: PositionalArgument] string Operation
) : GcloudOptions;