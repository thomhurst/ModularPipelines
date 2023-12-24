using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "get-agent-alias")]
public record AwsBedrockAgentGetAgentAliasOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--agent-alias-id")] string AgentAliasId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}