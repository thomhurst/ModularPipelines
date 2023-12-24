using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "list-connect-peers")]
public record AwsNetworkmanagerListConnectPeersOptions : AwsOptions
{
    [CommandSwitch("--core-network-id")]
    public string? CoreNetworkId { get; set; }

    [CommandSwitch("--connect-attachment-id")]
    public string? ConnectAttachmentId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}