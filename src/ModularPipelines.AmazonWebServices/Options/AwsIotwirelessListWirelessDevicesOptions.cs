using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotwireless", "list-wireless-devices")]
public record AwsIotwirelessListWirelessDevicesOptions : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--destination-name")]
    public string? DestinationName { get; set; }

    [CommandSwitch("--device-profile-id")]
    public string? DeviceProfileId { get; set; }

    [CommandSwitch("--service-profile-id")]
    public string? ServiceProfileId { get; set; }

    [CommandSwitch("--wireless-device-type")]
    public string? WirelessDeviceType { get; set; }

    [CommandSwitch("--fuota-task-id")]
    public string? FuotaTaskId { get; set; }

    [CommandSwitch("--multicast-group-id")]
    public string? MulticastGroupId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}