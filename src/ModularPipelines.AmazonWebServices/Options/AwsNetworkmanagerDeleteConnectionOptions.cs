using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "delete-connection")]
public record AwsNetworkmanagerDeleteConnectionOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}