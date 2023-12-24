using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "start-data-collection-by-agent-ids")]
public record AwsDiscoveryStartDataCollectionByAgentIdsOptions(
[property: CommandSwitch("--agent-ids")] string[] AgentIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}