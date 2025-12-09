using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "create-connect-attachment")]
public record AwsNetworkmanagerCreateConnectAttachmentOptions(
[property: CliOption("--core-network-id")] string CoreNetworkId,
[property: CliOption("--edge-location")] string EdgeLocation,
[property: CliOption("--transport-attachment-id")] string TransportAttachmentId,
[property: CliOption("--options")] string Options
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}