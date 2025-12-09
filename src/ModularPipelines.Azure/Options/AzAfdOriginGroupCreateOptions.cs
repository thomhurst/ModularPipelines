using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "origin-group", "create")]
public record AzAfdOriginGroupCreateOptions(
[property: CliOption("--additional-latency-in-milliseconds")] string AdditionalLatencyInMilliseconds,
[property: CliOption("--origin-group-name")] string OriginGroupName,
[property: CliOption("--probe-path")] string ProbePath,
[property: CliOption("--probe-protocol")] string ProbeProtocol,
[property: CliOption("--probe-request-type")] string ProbeRequestType,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sample-size")] string SampleSize,
[property: CliOption("--successful-samples-required")] string SuccessfulSamplesRequired
) : AzOptions
{
    [CliOption("--probe-interval-in-seconds")]
    public string? ProbeIntervalInSeconds { get; set; }
}