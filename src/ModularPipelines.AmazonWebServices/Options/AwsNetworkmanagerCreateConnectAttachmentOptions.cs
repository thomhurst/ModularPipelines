using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "create-connect-attachment")]
public record AwsNetworkmanagerCreateConnectAttachmentOptions(
[property: CommandSwitch("--core-network-id")] string CoreNetworkId,
[property: CommandSwitch("--edge-location")] string EdgeLocation,
[property: CommandSwitch("--transport-attachment-id")] string TransportAttachmentId,
[property: CommandSwitch("--options")] string Options
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}