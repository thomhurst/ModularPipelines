using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "associate-link")]
public record AwsNetworkmanagerAssociateLinkOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--link-id")] string LinkId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}