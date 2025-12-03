using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "http-health-checks", "delete")]
public record GcloudComputeHttpHealthChecksDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;