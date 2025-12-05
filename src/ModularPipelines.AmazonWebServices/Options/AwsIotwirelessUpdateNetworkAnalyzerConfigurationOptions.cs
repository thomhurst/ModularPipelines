using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-network-analyzer-configuration")]
public record AwsIotwirelessUpdateNetworkAnalyzerConfigurationOptions(
[property: CliOption("--configuration-name")] string ConfigurationName
) : AwsOptions
{
    [CliOption("--trace-content")]
    public string? TraceContent { get; set; }

    [CliOption("--wireless-devices-to-add")]
    public string[]? WirelessDevicesToAdd { get; set; }

    [CliOption("--wireless-devices-to-remove")]
    public string[]? WirelessDevicesToRemove { get; set; }

    [CliOption("--wireless-gateways-to-add")]
    public string[]? WirelessGatewaysToAdd { get; set; }

    [CliOption("--wireless-gateways-to-remove")]
    public string[]? WirelessGatewaysToRemove { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--multicast-groups-to-add")]
    public string[]? MulticastGroupsToAdd { get; set; }

    [CliOption("--multicast-groups-to-remove")]
    public string[]? MulticastGroupsToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}