using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "stop-data-collection-by-agent-ids")]
public record AwsDiscoveryStopDataCollectionByAgentIdsOptions(
[property: CommandSwitch("--agent-ids")] string[] AgentIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}