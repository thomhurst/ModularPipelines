using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric", "tap", "create")]
public record AzNetworkfabricTapCreateOptions(
[property: CommandSwitch("--destinations")] string Destinations,
[property: CommandSwitch("--network-packet-broker-id")] string NetworkPacketBrokerId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AzOptions
{
    [CommandSwitch("--annotation")]
    public string? Annotation { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--polling-type")]
    public string? PollingType { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}