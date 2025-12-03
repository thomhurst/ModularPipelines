using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "https-health-checks", "create")]
public record GcloudComputeHttpsHealthChecksCreateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--check-interval")]
    public string? CheckInterval { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--healthy-threshold")]
    public string? HealthyThreshold { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--request-path")]
    public string? RequestPath { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--unhealthy-threshold")]
    public string? UnhealthyThreshold { get; set; }
}