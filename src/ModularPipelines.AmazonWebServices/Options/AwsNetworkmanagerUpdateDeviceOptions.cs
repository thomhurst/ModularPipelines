using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "update-device")]
public record AwsNetworkmanagerUpdateDeviceOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--device-id")] string DeviceId
) : AwsOptions
{
    [CliOption("--aws-location")]
    public string? AwsLocation { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--vendor")]
    public string? Vendor { get; set; }

    [CliOption("--model")]
    public string? Model { get; set; }

    [CliOption("--serial-number")]
    public string? SerialNumber { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--site-id")]
    public string? SiteId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}