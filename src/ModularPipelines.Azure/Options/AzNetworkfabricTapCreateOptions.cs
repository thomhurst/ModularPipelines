using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkfabric", "tap", "create")]
public record AzNetworkfabricTapCreateOptions(
[property: CliOption("--destinations")] string Destinations,
[property: CliOption("--network-packet-broker-id")] string NetworkPacketBrokerId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName
) : AzOptions
{
    [CliOption("--annotation")]
    public string? Annotation { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--polling-type")]
    public string? PollingType { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}