using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "operations", "describe")]
public record GcloudEndpointsOperationsDescribeOptions(
[property: CliArgument] string Operation
) : GcloudOptions;