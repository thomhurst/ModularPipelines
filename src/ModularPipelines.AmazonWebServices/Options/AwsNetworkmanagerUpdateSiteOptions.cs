using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "update-site")]
public record AwsNetworkmanagerUpdateSiteOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--site-id")] string SiteId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}