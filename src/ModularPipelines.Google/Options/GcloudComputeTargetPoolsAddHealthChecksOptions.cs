using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-pools", "add-health-checks")]
public record GcloudComputeTargetPoolsAddHealthChecksOptions(
[property: CliArgument] string Name,
[property: CliOption("--http-health-check")] string HttpHealthCheck
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}