using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "origin-group", "update")]
public record AzAfdOriginGroupUpdateOptions : AzOptions
{
    [CommandSwitch("--additional-latency-in-milliseconds")]
    public string? AdditionalLatencyInMilliseconds { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--origin-group-name")]
    public string? OriginGroupName { get; set; }

    [CommandSwitch("--probe-interval-in-seconds")]
    public string? ProbeIntervalInSeconds { get; set; }

    [CommandSwitch("--probe-path")]
    public string? ProbePath { get; set; }

    [CommandSwitch("--probe-protocol")]
    public string? ProbeProtocol { get; set; }

    [CommandSwitch("--probe-request-type")]
    public string? ProbeRequestType { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sample-size")]
    public string? SampleSize { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--successful-samples-required")]
    public string? SuccessfulSamplesRequired { get; set; }
}