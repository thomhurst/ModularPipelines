using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "get-agent-action-group")]
public record AwsBedrockAgentGetAgentActionGroupOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--agent-version")] string AgentVersion,
[property: CommandSwitch("--action-group-id")] string ActionGroupId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}