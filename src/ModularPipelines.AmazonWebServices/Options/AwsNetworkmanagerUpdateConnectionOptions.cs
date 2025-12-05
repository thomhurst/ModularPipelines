using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "update-connection")]
public record AwsNetworkmanagerUpdateConnectionOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CliOption("--link-id")]
    public string? LinkId { get; set; }

    [CliOption("--connected-link-id")]
    public string? ConnectedLinkId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}