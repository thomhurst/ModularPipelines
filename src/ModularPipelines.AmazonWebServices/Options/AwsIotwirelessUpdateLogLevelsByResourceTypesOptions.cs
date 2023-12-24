using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "update-log-levels-by-resource-types")]
public record AwsIotwirelessUpdateLogLevelsByResourceTypesOptions : AwsOptions
{
    [CommandSwitch("--default-log-level")]
    public string? DefaultLogLevel { get; set; }

    [CommandSwitch("--wireless-device-log-options")]
    public string[]? WirelessDeviceLogOptions { get; set; }

    [CommandSwitch("--wireless-gateway-log-options")]
    public string[]? WirelessGatewayLogOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}