using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "start-data-collection-by-agent-ids")]
public record AwsDiscoveryStartDataCollectionByAgentIdsOptions(
[property: CliOption("--agent-ids")] string[] AgentIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}