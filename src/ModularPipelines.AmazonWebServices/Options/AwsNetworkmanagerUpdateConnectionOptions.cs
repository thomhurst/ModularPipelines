using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "update-connection")]
public record AwsNetworkmanagerUpdateConnectionOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId,
[property: CommandSwitch("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CommandSwitch("--link-id")]
    public string? LinkId { get; set; }

    [CommandSwitch("--connected-link-id")]
    public string? ConnectedLinkId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}