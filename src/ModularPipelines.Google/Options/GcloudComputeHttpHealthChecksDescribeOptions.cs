using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "http-health-checks", "describe")]
public record GcloudComputeHttpHealthChecksDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;