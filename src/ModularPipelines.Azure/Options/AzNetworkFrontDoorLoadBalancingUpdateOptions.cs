using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "front-door", "load-balancing", "update")]
public record AzNetworkFrontDoorLoadBalancingUpdateOptions(
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--additional-latency")]
    public string? AdditionalLatency { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--sample-size")]
    public string? SampleSize { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--successful-samples-required")]
    public string? SuccessfulSamplesRequired { get; set; }
}