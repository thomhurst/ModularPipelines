using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "update-agent-knowledge-base")]
public record AwsBedrockAgentUpdateAgentKnowledgeBaseOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-version")] string AgentVersion,
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--knowledge-base-state")]
    public string? KnowledgeBaseState { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}