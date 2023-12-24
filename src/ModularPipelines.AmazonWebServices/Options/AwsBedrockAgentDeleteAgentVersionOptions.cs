using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "delete-agent-version")]
public record AwsBedrockAgentDeleteAgentVersionOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--agent-version")] string AgentVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}