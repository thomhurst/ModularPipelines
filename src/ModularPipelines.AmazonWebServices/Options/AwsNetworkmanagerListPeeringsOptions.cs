using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "list-peerings")]
public record AwsNetworkmanagerListPeeringsOptions : AwsOptions
{
    [CliOption("--core-network-id")]
    public string? CoreNetworkId { get; set; }

    [CliOption("--peering-type")]
    public string? PeeringType { get; set; }

    [CliOption("--edge-location")]
    public string? EdgeLocation { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}