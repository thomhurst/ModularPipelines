using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "https-health-checks", "delete")]
public record GcloudComputeHttpsHealthChecksDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;