using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "get-connect-peer-associations")]
public record AwsNetworkmanagerGetConnectPeerAssociationsOptions(
[property: CommandSwitch("--global-network-id")] string GlobalNetworkId
) : AwsOptions
{
    [CommandSwitch("--connect-peer-ids")]
    public string[]? ConnectPeerIds { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}