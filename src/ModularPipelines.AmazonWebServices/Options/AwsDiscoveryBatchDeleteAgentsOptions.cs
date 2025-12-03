using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("discovery", "batch-delete-agents")]
public record AwsDiscoveryBatchDeleteAgentsOptions(
[property: CliOption("--delete-agents")] string[] DeleteAgents
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}