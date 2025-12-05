using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "delete-site")]
public record AwsNetworkmanagerDeleteSiteOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--site-id")] string SiteId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}