using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "load-balancing", "create")]
public record AzNetworkFrontDoorLoadBalancingCreateOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sample-size")] string SampleSize,
[property: CliOption("--successful-samples-required")] string SuccessfulSamplesRequired
) : AzOptions
{
    [CliOption("--additional-latency")]
    public string? AdditionalLatency { get; set; }
}