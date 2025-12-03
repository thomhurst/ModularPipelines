using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-pools", "remove-health-checks")]
public record GcloudComputeTargetPoolsRemoveHealthChecksOptions(
[property: CliArgument] string Name,
[property: CliOption("--http-health-check")] string HttpHealthCheck
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}