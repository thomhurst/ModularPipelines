using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("discovery", "batch-delete-agents")]
public record AwsDiscoveryBatchDeleteAgentsOptions(
[property: CommandSwitch("--delete-agents")] string[] DeleteAgents
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}