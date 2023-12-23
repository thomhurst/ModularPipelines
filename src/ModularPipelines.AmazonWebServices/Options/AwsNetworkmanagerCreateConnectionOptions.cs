using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "create-connection")]
public record AwsNetworkmanagerCreateConnectionOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId,
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--connected-device-id")] string ConnectedDeviceId
) : AwsOptions
{
    [CommandSwitch("--link-id")]
    public string? LinkId { get; set; }

    [CommandSwitch("--connected-link-id")]
    public string? ConnectedLinkId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}