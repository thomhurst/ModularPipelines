using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "start-route-analysis")]
public record AwsNetworkmanagerStartRouteAnalysisOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--source")] string Source,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}