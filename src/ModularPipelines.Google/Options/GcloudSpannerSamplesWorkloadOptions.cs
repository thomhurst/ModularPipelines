using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "samples", "workload")]
public record GcloudSpannerSamplesWorkloadOptions(
[property: PositionalArgument] string Appname
) : GcloudOptions
{
    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--target-qps")]
    public string? TargetQps { get; set; }
}