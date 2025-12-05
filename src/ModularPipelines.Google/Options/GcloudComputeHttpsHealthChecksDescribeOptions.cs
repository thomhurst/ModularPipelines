using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "https-health-checks", "describe")]
public record GcloudComputeHttpsHealthChecksDescribeOptions(
[property: CliArgument] string Name
) : GcloudOptions;