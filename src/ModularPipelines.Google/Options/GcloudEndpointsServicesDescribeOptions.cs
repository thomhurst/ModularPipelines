using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("endpoints", "services", "describe")]
public record GcloudEndpointsServicesDescribeOptions(
[property: PositionalArgument] string Service
) : GcloudOptions;