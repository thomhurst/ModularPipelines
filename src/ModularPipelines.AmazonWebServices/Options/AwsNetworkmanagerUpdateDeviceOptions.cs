using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "update-device")]
public record AwsNetworkmanagerUpdateDeviceOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AwsOptions
{
    [CommandSwitch("--aws-location")]
    public string? AwsLocation { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--vendor")]
    public string? Vendor { get; set; }

    [CommandSwitch("--model")]
    public string? Model { get; set; }

    [CommandSwitch("--serial-number")]
    public string? SerialNumber { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--site-id")]
    public string? SiteId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}