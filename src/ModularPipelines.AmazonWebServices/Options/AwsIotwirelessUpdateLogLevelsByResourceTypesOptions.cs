using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "update-log-levels-by-resource-types")]
public record AwsIotwirelessUpdateLogLevelsByResourceTypesOptions : AwsOptions
{
    [CliOption("--default-log-level")]
    public string? DefaultLogLevel { get; set; }

    [CliOption("--wireless-device-log-options")]
    public string[]? WirelessDeviceLogOptions { get; set; }

    [CliOption("--wireless-gateway-log-options")]
    public string[]? WirelessGatewayLogOptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}