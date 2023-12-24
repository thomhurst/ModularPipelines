using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "update-agent-knowledge-base")]
public record AwsBedrockAgentUpdateAgentKnowledgeBaseOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--agent-version")] string AgentVersion,
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--knowledge-base-state")]
    public string? KnowledgeBaseState { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}