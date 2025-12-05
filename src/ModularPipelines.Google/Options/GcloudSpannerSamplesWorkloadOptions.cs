using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "samples", "workload")]
public record GcloudSpannerSamplesWorkloadOptions(
[property: CliArgument] string Appname
) : GcloudOptions
{
    [CliOption("--duration")]
    public string? Duration { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--target-qps")]
    public string? TargetQps { get; set; }
}