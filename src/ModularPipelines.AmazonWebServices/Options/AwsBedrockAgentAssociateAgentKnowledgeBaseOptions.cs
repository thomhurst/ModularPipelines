using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "associate-agent-knowledge-base")]
public record AwsBedrockAgentAssociateAgentKnowledgeBaseOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-version")] string AgentVersion,
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--description")] string Description
) : AwsOptions
{
    [CliOption("--knowledge-base-state")]
    public string? KnowledgeBaseState { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}