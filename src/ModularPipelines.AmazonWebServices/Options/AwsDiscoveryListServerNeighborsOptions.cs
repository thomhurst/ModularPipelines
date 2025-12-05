using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "list-server-neighbors")]
public record AwsDiscoveryListServerNeighborsOptions(
[property: CliOption("--configuration-id")] string ConfigurationId
) : AwsOptions
{
    [CliOption("--neighbor-configuration-ids")]
    public string[]? NeighborConfigurationIds { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}