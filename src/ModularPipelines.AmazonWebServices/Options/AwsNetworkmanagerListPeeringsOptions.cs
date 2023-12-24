using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkmanager", "list-peerings")]
public record AwsNetworkmanagerListPeeringsOptions : AwsOptions
{
    [CommandSwitch("--core-network-id")]
    public string? CoreNetworkId { get; set; }

    [CommandSwitch("--peering-type")]
    public string? PeeringType { get; set; }

    [CommandSwitch("--edge-location")]
    public string? EdgeLocation { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}