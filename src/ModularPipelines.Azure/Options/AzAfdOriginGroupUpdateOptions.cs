using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("afd", "origin-group", "update")]
public record AzAfdOriginGroupUpdateOptions : AzOptions
{
    [CliOption("--additional-latency-in-milliseconds")]
    public string? AdditionalLatencyInMilliseconds { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--origin-group-name")]
    public string? OriginGroupName { get; set; }

    [CliOption("--probe-interval-in-seconds")]
    public string? ProbeIntervalInSeconds { get; set; }

    [CliOption("--probe-path")]
    public string? ProbePath { get; set; }

    [CliOption("--probe-protocol")]
    public string? ProbeProtocol { get; set; }

    [CliOption("--probe-request-type")]
    public string? ProbeRequestType { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sample-size")]
    public string? SampleSize { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--successful-samples-required")]
    public string? SuccessfulSamplesRequired { get; set; }
}