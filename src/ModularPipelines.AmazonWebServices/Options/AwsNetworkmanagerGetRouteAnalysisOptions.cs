using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "get-route-analysis")]
public record AwsNetworkmanagerGetRouteAnalysisOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--route-analysis-id")] string RouteAnalysisId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}