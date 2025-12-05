using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "create-connection")]
public record AwsNetworkmanagerCreateConnectionOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--connected-device-id")] string ConnectedDeviceId
) : AwsOptions
{
    [CliOption("--link-id")]
    public string? LinkId { get; set; }

    [CliOption("--connected-link-id")]
    public string? ConnectedLinkId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}