using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("endpoints", "services", "describe")]
public record GcloudEndpointsServicesDescribeOptions(
[property: CliArgument] string Service
) : GcloudOptions;