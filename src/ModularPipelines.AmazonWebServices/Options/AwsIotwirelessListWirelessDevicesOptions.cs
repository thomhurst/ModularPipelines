using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotwireless", "list-wireless-devices")]
public record AwsIotwirelessListWirelessDevicesOptions : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--destination-name")]
    public string? DestinationName { get; set; }

    [CliOption("--device-profile-id")]
    public string? DeviceProfileId { get; set; }

    [CliOption("--service-profile-id")]
    public string? ServiceProfileId { get; set; }

    [CliOption("--wireless-device-type")]
    public string? WirelessDeviceType { get; set; }

    [CliOption("--fuota-task-id")]
    public string? FuotaTaskId { get; set; }

    [CliOption("--multicast-group-id")]
    public string? MulticastGroupId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}