using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "origin-group", "create")]
public record AzAfdOriginGroupCreateOptions(
[property: CommandSwitch("--additional-latency-in-milliseconds")] string AdditionalLatencyInMilliseconds,
[property: CommandSwitch("--origin-group-name")] string OriginGroupName,
[property: CommandSwitch("--probe-path")] string ProbePath,
[property: CommandSwitch("--probe-protocol")] string ProbeProtocol,
[property: CommandSwitch("--probe-request-type")] string ProbeRequestType,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sample-size")] string SampleSize,
[property: CommandSwitch("--successful-samples-required")] string SuccessfulSamplesRequired
) : AzOptions
{
    [CommandSwitch("--probe-interval-in-seconds")]
    public string? ProbeIntervalInSeconds { get; set; }
}

