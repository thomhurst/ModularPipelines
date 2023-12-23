using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "update-network-analyzer-configuration")]
public record AwsIotwirelessUpdateNetworkAnalyzerConfigurationOptions(
[property: CommandSwitch("--configuration-name")] string ConfigurationName
) : AwsOptions
{
    [CommandSwitch("--trace-content")]
    public string? TraceContent { get; set; }

    [CommandSwitch("--wireless-devices-to-add")]
    public string[]? WirelessDevicesToAdd { get; set; }

    [CommandSwitch("--wireless-devices-to-remove")]
    public string[]? WirelessDevicesToRemove { get; set; }

    [CommandSwitch("--wireless-gateways-to-add")]
    public string[]? WirelessGatewaysToAdd { get; set; }

    [CommandSwitch("--wireless-gateways-to-remove")]
    public string[]? WirelessGatewaysToRemove { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--multicast-groups-to-add")]
    public string[]? MulticastGroupsToAdd { get; set; }

    [CommandSwitch("--multicast-groups-to-remove")]
    public string[]? MulticastGroupsToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}