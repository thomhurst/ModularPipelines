using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "get-site-to-site-vpn-attachment")]
public record AwsNetworkmanagerGetSiteToSiteVpnAttachmentOptions(
[property: CliOption("--attachment-id")] string AttachmentId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}