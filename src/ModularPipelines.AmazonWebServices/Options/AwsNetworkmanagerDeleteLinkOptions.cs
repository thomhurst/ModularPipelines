using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "delete-link")]
public record AwsNetworkmanagerDeleteLinkOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--link-id")] string LinkId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}